using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IAvailabilitiesRepository : IBaseRepository<Availability>
    {
        Task<OperationResult<List<Availability>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult<List<Availability>>> GetActiveByDoctorIdAsync(int doctorId);
    }
}
