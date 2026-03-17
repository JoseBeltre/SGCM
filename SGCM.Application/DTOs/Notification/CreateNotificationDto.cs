using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Notification
{
    public record CreateNotificationDto
    {
        public required int AppointmentId { get; init; }
        public required int UserId { get; init; }
        public required string EventType{ get; init; }
        public string? Subject { get; init; }
        public required string Message { get; init; }
        public NotificationType? NotificationType { get; init; } = null;
    }
}
