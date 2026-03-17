using SGCM.Application.DTOs.Notification;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces.Notification
{
    public interface INotificationService
    {
        Task<OperationResult<NotificationResponse>> CreateAndSendAsync(CreateNotificationDto createNotificationDto);

        Task<OperationResult> SendAsync(int notificationId);
    }
}
