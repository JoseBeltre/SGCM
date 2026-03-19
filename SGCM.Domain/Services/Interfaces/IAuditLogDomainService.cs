using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IAuditLogDomainService
    {
        Task<OperationResult<AuditLog>> RecordCreateAsync<T>(
            int userId,
            EntityType entityType,
            int entityId,
            T newEntity,
            string? ipAddress = null,
            string? userAgent = null);

        Task<OperationResult<AuditLog>> RecordUpdateAsync<T>(
            int userId,
            EntityType entityType,
            int entityId,
            T previousEntity,
            T newEntity,
            string? ipAddress = null,
            string? userAgent = null);

        Task<OperationResult<AuditLog>> RecordDeleteAsync<T>(
            int userId,
            EntityType entityType,
            int entityId,
            T previousEntity,
            string? ipAddress = null,
            string? userAgent = null);

        Task<OperationResult<AuditLog>> RecordCustomAsync(
            int userId,
            EntityType entityType,
            int entityId,
            AuditAction action,
            object? previousValues = null,
            object? newValues = null,
            string? ipAddress = null,
            string? userAgent = null);
    }
}
