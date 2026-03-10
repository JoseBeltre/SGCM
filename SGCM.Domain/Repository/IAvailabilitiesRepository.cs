using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IAvailabilitiesRepository : IBaseRepository<Availabilities>
    {
        Task<OperationResult<List<Availabilities>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult<List<Availabilities>>> GetActiveByDoctorIdAsync(int doctorId);
    }
}
