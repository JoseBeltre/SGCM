using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IAvailabilityExceptionRepository : IBaseRepository<AvailabilityExceptions>
    {
        Task<OperationResult<List<AvailabilityExceptions>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult<List<AvailabilityExceptions>>> GetByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate);
    }
}
