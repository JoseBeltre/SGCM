using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IAppointmentHistoryRepository : IBaseRepository<AppointmentHistory>
    {
        Task<OperationResult<List<AppointmentHistory>>> GetByAppointmentIdAsync(int appointmentId);
    }
}