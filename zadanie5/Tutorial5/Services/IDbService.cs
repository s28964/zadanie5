using Tutorial5.DTOs;

namespace Tutorial5.Services;

public interface IDbService
{
    Task AddPrescriptionAsync(AddPrescriptionDto prescription);
    Task<PatientDetailsDto> GetPatientAsync(int id);
}