using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Repository
{
    public interface INotificationsRepository : IBaseRepository<Notification>
    {
        Task<OperationResult<List<Notification>>> GetByUserIdAsync(int userId);
        Task<OperationResult<List<Notification>>> GetByAppointmentIdAsync(int appointmentId);
        Task<OperationResult<List<Notification>>> GetByStatusAsync(NotificationStatus status);
        Task<OperationResult<List<Notification>>> GetByTypeAsync(NotificationType type);
        Task<OperationResult<List<Notification>>> GetByEventTypeAsync(string eventType);
    }
}
