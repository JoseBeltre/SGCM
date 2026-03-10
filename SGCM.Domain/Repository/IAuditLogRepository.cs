using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Repository
{
    public interface IAuditLogRepository
    {
        Task<OperationResult> AddAsync(AuditLog auditLog);
        Task<OperationResult<AuditLog>> GetByIdAsync(int logId);
        Task<OperationResult<List<AuditLog>>> GetByUserIdAsync(int userId);
        Task<OperationResult<List<AuditLog>>> GetByEntityAsync(EntityType entityType, int entityId);
        Task<OperationResult<List<AuditLog>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
