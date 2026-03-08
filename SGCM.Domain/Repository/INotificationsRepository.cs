using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Entities.Enums;

namespace SGCM.Domain.Repository
{
    public interface INotificationsRepository : IBaseRepository<Notifications>
    {
        Task<OperationResult<List<Notifications>>> GetByUserIdAsync(int userId);
        Task<OperationResult<List<Notifications>>> GetByAppointmentIdAsync(int appointmentId);
        Task<OperationResult<List<Notifications>>> GetByStatusAsync(NotificationStatus status);
        Task<OperationResult<List<Notifications>>> GetByTypeAsync(NotificationType type);
    }
}
