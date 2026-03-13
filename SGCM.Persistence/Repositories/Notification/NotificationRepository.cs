using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;

namespace SGCM.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        public Task<OperationResult<Notification>> AddAsync(Notification entity)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Notification>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Notification>>> GetByAppointmentIdAsync(int appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Notification>>> GetByEventTypeAsync(string eventType)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Notification?>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Notification>>> GetByStatusAsync(NotificationStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Notification>>> GetByTypeAsync(NotificationType type)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Notification>>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Notification?>> UpdateAsync(Notification entity)
        {
            throw new NotImplementedException();
        }
    }
}
