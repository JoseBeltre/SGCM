using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IPatientService
    {
        Task<OperationResult<bool>> IsEligibleForAppointmentAsync(int patientId);
    }
}