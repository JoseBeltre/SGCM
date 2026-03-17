

using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    using System.Text.Json;

    public class AuditLogDomainService : IAuditLogDomainService
    {
        private readonly IAuditLogRepository _auditRepository;
        private readonly IUsersRepository _usersRepository;

        public AuditLogDomainService(
            IAuditLogRepository auditRepository,
            IUsersRepository usersRepository)
        {
            _auditRepository = auditRepository;
            _usersRepository = usersRepository;
        }

        public async Task<OperationResult<AuditLog>> RecordCreateAsync<T>(
            int userId,
            EntityType entityType,
            int entityId,
            T newEntity,
            string? ipAddress = null,
            string? userAgent = null)
        {
            return await CreateLogAsync(
                userId,
                entityType,
                entityId,
                AuditAction.Create,
                null,
                newEntity,
                ipAddress,
                userAgent);
        }

        public async Task<OperationResult<AuditLog>> RecordUpdateAsync<T>(
            int userId,
            EntityType entityType,
            int entityId,
            T previousEntity,
            T newEntity,
            string? ipAddress = null,
            string? userAgent = null)
        {
            return await CreateLogAsync(
                userId,
                entityType,
                entityId,
                AuditAction.Update,
                previousEntity,
                newEntity,
                ipAddress,
                userAgent);
        }

        public async Task<OperationResult<AuditLog>> RecordDeleteAsync<T>(
            int userId,
            EntityType entityType,
            int entityId,
            T previousEntity,
            string? ipAddress = null,
            string? userAgent = null)
        {
            return await CreateLogAsync(
                userId,
                entityType,
                entityId,
                AuditAction.Delete,
                previousEntity,
                null,
                ipAddress,
                userAgent);
        }

        public async Task<OperationResult<AuditLog>> RecordCustomAsync(
            int userId,
            EntityType entityType,
            int entityId,
            AuditAction action,
            object? previousValues = null,
            object? newValues = null,
            string? ipAddress = null,
            string? userAgent = null)
        {
            return await CreateLogAsync(
                userId,
                entityType,
                entityId,
                action,
                previousValues,
                newValues,
                ipAddress,
                userAgent);
        }

        // Metodo general para crear los logs
        private async Task<OperationResult<AuditLog>> CreateLogAsync(
            int userId,
            EntityType entityType,
            int entityId,
            AuditAction action,
            object? previousValues,
            object? newValues,
            string? ipAddress,
            string? userAgent)
        {
            // Validar que ids sean validos y que el usuario exista
            if (userId <= 0)
                return OperationResult<AuditLog>.Failure("Invalid user");

            if (entityId <= 0)
                return OperationResult<AuditLog>.Failure("Invalid entity id");

            var userExists = await _usersRepository.ExistsAsync(userId);
            if (!userExists.IsSuccess || !userExists.Data)
                return OperationResult<AuditLog>.Failure("User not found");

            // Serializacion (convertir a JSON)
            string? previousJson = previousValues is null
                ? null
                : JsonSerializer.Serialize(previousValues);

            string? newJson = newValues is null
                ? null
                : JsonSerializer.Serialize(newValues);

            var log = new AuditLog
            {
                UserId = userId,
                EntityType = entityType,
                EntityId = entityId,
                Action = action,
                PreviousValues = previousJson,
                NewValues = newJson,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ActionDate = DateTime.UtcNow
            };

            var saved = await _auditRepository.AddAsync(log);

            if (!saved.IsSuccess)
                return OperationResult<AuditLog>.Failure("Failed to record audit log");

            return OperationResult<AuditLog>.Success(saved.Data);
        }
    }
}
