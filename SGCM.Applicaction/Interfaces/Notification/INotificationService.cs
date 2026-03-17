using SGCM.Applicaction.DTOs.Notification;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces.Notification
{
    public interface INotificationService
    {
        Task<OperationResult<NotificationResponse>> CreateAndSendAsync(CreateNotificationDto createNotificationDto);

        Task<OperationResult> SendAsync(int notificationId);
    }
}
