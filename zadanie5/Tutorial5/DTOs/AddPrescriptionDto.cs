using Tutorial5.Models;

namespace Tutorial5.DTOs;

public class AddPrescriptionDto
{
    public PatientDto Patient { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescribedMedicamentDto> Medicaments { get; set; }
}

public class PatientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
}

public class PrescribedMedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}