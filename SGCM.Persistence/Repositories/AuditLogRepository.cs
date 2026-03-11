using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;

namespace SGCM.Persistence.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        public Task<OperationResult<AuditLog>> AddAsync(AuditLog auditLog)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<AuditLog>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<AuditLog>>> GetByEntityAsync(EntityType entityType, int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<AuditLog>> GetByIdAsync(int logId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<AuditLog>>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
