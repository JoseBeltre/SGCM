using Microsoft.Extensions.Logging;
using SGCM.Applicaction.DTOs.Notification;
using SGCM.Applicaction.Interfaces.Notification;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Applicaction.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationDomainService _notificationDomainService;
        private readonly ILogger<NotificationService> _logger;
        private readonly IAuditLogDomainService _auditLogDomainService;
        private readonly IUsersRepository _usersRepository;

        public NotificationService(
            INotificationRepository notificationsRepository,
            INotificationDomainService notificationDomainService,
            IAuditLogDomainService auditLogDomainService,
            ILogger<NotificationService> logger,
            IUsersRepository usersRepository)
        {
            _notificationRepository = notificationsRepository;
            _notificationDomainService = notificationDomainService;
            _auditLogDomainService = auditLogDomainService;
            _logger = logger;
            _usersRepository = usersRepository;
        }

        public async Task<OperationResult<NotificationResponse>> CreateAndSendAsync(CreateNotificationDto createNotificationDto)
        {
            try
            {
                var result = await _notificationDomainService.CreateAsync(
                    appointmentId: createNotificationDto.AppointmentId,
                    userId: createNotificationDto.UserId,
                    eventType: createNotificationDto.EventType,
                    subject: createNotificationDto.Subject,
                    message: createNotificationDto.Message,
                    type: createNotificationDto.NotificationType ?? NotificationType.Email
                    );
                
                if (!result.IsSuccess)
                    return OperationResult<NotificationResponse>.Failure(result.Message);

                _logger.LogInformation("Notification created with ID {NotificationId} for User {UserId} and Appointment {AppointmentId}", result.Data.Id, result.Data.UserId, result.Data.AppointmentId);
                

                await _auditLogDomainService.RecordCreateAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Specialty,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                    );

                // Simulate sending the notification (e.g., via email or SMS)
                _logger.LogInformation("Sending notification ID {NotificationId} to User {UserId}", result.Data.Id, result.Data.UserId);
                
                var sendResult = await SendAsync(result.Data.Id);
                if (!sendResult.IsSuccess)
                    return OperationResult<NotificationResponse>.Failure($"Notification created but failed to send: {sendResult.Message}");

                return OperationResult<NotificationResponse>.Success(NotificationMapper.ToResponse(result.Data));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating and sending notification");
                return OperationResult<NotificationResponse>.Failure("An error occurred while creating and sending the notification.");
            }
        }

        public async Task<OperationResult> SendAsync(int notificationId)
        {
            try
            {
            var notificationResult = await _notificationRepository.GetByIdAsync(notificationId);

            if (!notificationResult.IsSuccess)
                return OperationResult.Failure(notificationResult.Message);

            var notification = notificationResult.Data;

            var userResult = await _usersRepository.GetByIdAsync(notification.UserId);

            if (!userResult.IsSuccess)
                return OperationResult.Failure(notificationResult.Message);

            var user = userResult.Data;

                if (notification.NotificationType == NotificationType.Email ||
                    notification.NotificationType == NotificationType.Both)
                {
                    _logger.LogInformation("Sending EMAIL notification to {User}", user.Email);
                }

                if (notification.NotificationType == NotificationType.SMS ||
                    notification.NotificationType == NotificationType.Both)
                {
                    _logger.LogInformation("Sending SMS notification to {User}", user.Phone);
                }

                var markSent = await _notificationDomainService.MarkAsSentAsync(notification.Id);

                if (!markSent.IsSuccess)
                    return OperationResult.Failure(markSent.Message);

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification {Id}", notificationId);
                return OperationResult.Failure("Failed to send notification");
            }
        }
    }
}
