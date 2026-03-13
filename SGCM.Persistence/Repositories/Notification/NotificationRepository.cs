using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<List<Notification>>> GetAllAsync()
        {
            var notifications = await _context.Notifications.ToListAsync();
            return OperationResult<List<Notification>>.Success(notifications);
        }

        public async Task<OperationResult<List<Notification>>> GetByAppointmentIdAsync(int appointmentId)
        {
            var notifications = await _context.Notifications.Where(n => n.AppointmentId == appointmentId).ToListAsync();
            return OperationResult<List<Notification>>.Success(notifications);
        }

        public async Task<OperationResult<List<Notification>>> GetByEventTypeAsync(string eventType)
        {
            var notifications = await _context.Notifications.Where(n => n.EventType == eventType).ToListAsync();
            return OperationResult<List<Notification>>.Success(notifications);
        }

        public async Task<OperationResult<Notification?>> GetByIdAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            return OperationResult<Notification?>.Success(notification);
        }

        public async Task<OperationResult<List<Notification>>> GetByStatusAsync(NotificationStatus status)
        {
            var notifications = await _context.Notifications.Where(n => n.Status == status).ToListAsync();
            return OperationResult<List<Notification>>.Success(notifications);
        }

        public async Task<OperationResult<List<Notification>>> GetByTypeAsync(NotificationType type)
        {
            var notifications = await _context.Notifications.Where(n => n.NotificationType == type).ToListAsync();
            return OperationResult<List<Notification>>.Success(notifications);
        }

        public async Task<OperationResult<List<Notification>>> GetByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
            return OperationResult<List<Notification>>.Success(notifications);
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            bool exists = await _context.Notifications.AnyAsync(n => n.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<Notification>> AddAsync(Notification entity)
        {
            await _context.Notifications.AddAsync(entity);
            await _context.SaveChangesAsync();
            return OperationResult<Notification>.Success(entity);
        }

        public async Task<OperationResult<Notification?>> UpdateAsync(Notification entity)
        {
            var existing = await _context.Notifications.FindAsync(entity.Id);
            if (existing == null)
                return OperationResult<Notification?>.Failure("Notification not found");

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return OperationResult<Notification?>.Success(existing);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var entity = await _context.Notifications.FindAsync(id);
            if (entity == null)
                return OperationResult<bool>.Failure("Notification not found");

            _context.Notifications.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}