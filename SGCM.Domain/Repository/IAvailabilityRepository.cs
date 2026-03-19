using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IAvailabilityRepository : IBaseRepository<Availability>
    {
        Task<OperationResult<List<Availability>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult<List<Availability>>> GetActiveByDoctorIdAsync(int doctorId);
    }
}
