using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IAvailabilityDomainService
    {
        Task<OperationResult<bool>> IsDoctorAvailableAsync(int doctorId, DateTime appointementDate, TimeSpan startTime, TimeSpan endTime);
    }
}
