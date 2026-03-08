using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Entities.Enums;

namespace SGCM.Domain.Repository
{
    public interface IAuditLogsRepository
    {
        Task<OperationResult> AddAsync(AuditLogs auditLog);
        Task<OperationResult<AuditLogs>> GetByIdAsync(int logId);
        Task<OperationResult<List<AuditLogs>>> GetByUserIdAsync(int userId);
        Task<OperationResult<List<AuditLogs>>> GetByEntityAsync(EntityType entityType, int entityId);
        Task<OperationResult<List<AuditLogs>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
