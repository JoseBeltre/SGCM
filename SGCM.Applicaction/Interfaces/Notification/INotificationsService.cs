using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;

namespace SGCM.Applicaction.Interfaces.Notification
{
    public interface INotificationsService
    {
        Task<OperationResult<Notification>> CreateAsync(int userId,
            int appointmentId,
            string eventType,
            string subject,
            string message,
            NotificationType? type);
        Task<OperationResult<bool>> MarkAsSentAsync(int notificationId);
    }
}
