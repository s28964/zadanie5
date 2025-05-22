using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;

namespace Tutorial5.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(AddPrescriptionDto prescription)
    {
        if(prescription.Medicaments.Count > 10)
            throw new ArgumentException("Medicaments count must be less than 10");
        
        if(prescription.DueDate < prescription.Date)
            throw new ArgumentException("Due date must be greater than or equal to prescription date");
        
        var medicamentIds = prescription.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament).ToListAsync();
        
        if (existingMedicaments.Count() != medicamentIds.Count)
            throw new ArgumentException("One or more medicaments don't exist");

        var patient = await _context.Patients.FirstOrDefaultAsync(p =>
            p.FirstName == prescription.Patient.FirstName && p.LastName == prescription.Patient.LastName &&
            p.Birthdate == prescription.Patient.Birthdate);
        
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = prescription.Patient.FirstName,
                LastName = prescription.Patient.LastName,
                Birthdate = prescription.Patient.Birthdate,
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var newPrescription = new Prescription
        {
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            IdDoctor = prescription.IdDoctor,
            IdPatient = patient.IdPatient
        };
        
        _context.Prescriptions.Add(newPrescription);
        await _context.SaveChangesAsync();

        foreach (var meds in prescription.Medicaments)
        {
            _context.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                IdPrescription = newPrescription.IdPrescription,
                IdMedicament = meds.IdMedicament,
                Dose = meds.Dose,
                Description = meds.Description,
            });
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<PatientDetailsDto> GetPatientAsync(int patientId)
    {
        var patient = await _context.Patients.Include(p => p.Prescriptions).ThenInclude(p => p.Doctor).Include(p => p.Prescriptions).ThenInclude(p => p.PrescriptionMedicaments).ThenInclude(pm => pm.Medicament).FirstOrDefaultAsync(p => p.IdPatient == patientId);
        
        if(patient == null)
            return null;

        return new PatientDetailsDto()
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate).Select(p => new PrescriptionDto
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new DoctorDto
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                },
                Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDto
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Description,
                }).ToList()
            }).ToList()
        };
    }
}