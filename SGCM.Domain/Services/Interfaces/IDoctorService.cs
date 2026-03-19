using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<OperationResult<bool>> CanBeDeactivatedAsync(int doctorId);
        Task<OperationResult<bool>> DeactivateAsync(int doctorId);
    }
}