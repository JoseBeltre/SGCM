using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IAvailabilityExceptionRepository : IBaseRepository<AvailabilityException>
    {
        Task<OperationResult<List<AvailabilityException>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult<List<AvailabilityException>>> GetByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate);
        Task<OperationResult<AvailabilityException?>> ExistsInDateAsync(int doctorId, DateTime appointmentDate);
        Task<OperationResult<bool>> ExistsConflictAsync(int doctorId, DateTime startDate, DateTime endDate);
    }
}
