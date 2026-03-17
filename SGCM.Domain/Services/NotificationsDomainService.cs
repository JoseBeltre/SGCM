using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    public class NotificationsDomainService : INotificationDomainService
    {
        private readonly INotificationRepository _notificationsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IAppointmentsRepository _appointmentsRepository;

        public NotificationsDomainService(INotificationRepository notificationsRepository, IUsersRepository usersRepository, IAppointmentsRepository appointmentsRepository)
        {
            _notificationsRepository = notificationsRepository;
            _usersRepository = usersRepository;
            _appointmentsRepository = appointmentsRepository;
        }

        public async Task<OperationResult<Notification>> CreateAsync(
            int userId,
            int appointmentId,
            string eventType,
            string subject,
            string message,
            NotificationType? type = null)
        {
            // Validar user existe
            var userExists = await _usersRepository.ExistsAsync(userId);
            if (!userExists.IsSuccess || !userExists.Data)
                return OperationResult<Notification>.Failure("User not found");

            // Validar cita existe
            var apptExists = await _appointmentsRepository.ExistsAsync(appointmentId);
            if (!apptExists.IsSuccess || !apptExists.Data)
                return OperationResult<Notification>.Failure("Appointment not found");

            var notification = new Notification
            {
                AppointmentId = appointmentId,
                UserId = userId,
                EventType = eventType,
                NotificationType = type ?? NotificationType.Both,
                Subject = subject,
                Message = message,
                Status = NotificationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _notificationsRepository.AddAsync(notification);
            if (!saved.IsSuccess)
                return OperationResult<Notification>.Failure("Failed to create notification");

            return OperationResult<Notification>.Success(saved.Data);
        }

        public async Task<OperationResult<bool>> MarkAsSentAsync(int notificationId)
        {
            var notificationResult = await _notificationsRepository.GetByIdAsync(notificationId);
            if (notificationResult == null || !notificationResult.IsSuccess)
                return OperationResult<bool>.Failure("Notification not found");

            var notification = notificationResult.Data;

            if (notification.Status != NotificationStatus.Pending)
                return OperationResult<bool>.Failure("Only pending notifications can be marked as sent");

            notification.Status = NotificationStatus.Sent;
            notification.SentAt = DateTime.UtcNow;
            notification.SendAttempts += 1;

            var updated = await _notificationsRepository.UpdateAsync(notification);
            if (!updated.IsSuccess)
                return OperationResult<bool>.Failure("Failed to update notification status");

            return OperationResult<bool>.Success(true);
        }
    }
}
