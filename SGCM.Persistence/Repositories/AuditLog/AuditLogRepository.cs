using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<AuditLog>> AddAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();

            return OperationResult<AuditLog>.Success(auditLog);
        }

        public async Task<OperationResult<List<AuditLog>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var logs = await _context.AuditLogs
                .Where(l => l.ActionDate >= startDate && l.ActionDate <= endDate)
                .ToListAsync();

            return OperationResult<List<AuditLog>>.Success(logs);
        }

        public async Task<OperationResult<List<AuditLog>>> GetByEntityAsync(EntityType entityType, int entityId)
        {
            var logs = await _context.AuditLogs
                .Where(l => l.EntityType == entityType && l.EntityId == entityId)
                .ToListAsync();

            return OperationResult<List<AuditLog>>.Success(logs);
        }

        public async Task<OperationResult<AuditLog>> GetByIdAsync(int logId)
        {
            var log = await _context.AuditLogs.FindAsync(logId);

            if (log == null)
                return OperationResult<AuditLog>.Failure("Audit log not found");

            return OperationResult<AuditLog>.Success(log);
        }

        public async Task<OperationResult<List<AuditLog>>> GetByUserIdAsync(int userId)
        {
            var logs = await _context.AuditLogs
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return OperationResult<List<AuditLog>>.Success(logs);
        }
    }
}