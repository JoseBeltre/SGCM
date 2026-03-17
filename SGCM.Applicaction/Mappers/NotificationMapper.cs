using SGCM.Applicaction.DTOs.Availability;
using SGCM.Applicaction.DTOs.Notification;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationResponse ToResponse(Notification notification)
        {
            return new NotificationResponse
            {
                Id = notification.Id,
                AppointmentId = notification.AppointmentId,
                UserId = notification.UserId,
                EventType = notification.EventType,
                Subject = notification.Subject,
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                Status = notification.Status,
                SentAt = notification.SentAt,
                SendAttempts = notification.SendAttempts
            };
        }
    }
}