using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Notification
{
    public record NotificationResponse
    {
        public required int Id { get; init; }
        public required int AppointmentId { get; init; }
        public required int UserId { get; init; }
        public required string EventType { get; init; }
        public string? Subject { get; init; }
        public required string Message { get; init; }
        public NotificationType NotificationType { get; init; }
        public NotificationStatus Status { get; init; }
        public DateTime SentAt { get; init; }
        public int SendAttempts { get; init; }
    }
}
