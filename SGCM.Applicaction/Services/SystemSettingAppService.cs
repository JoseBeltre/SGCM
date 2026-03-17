using Microsoft.Extensions.Logging;
using SGCM.Application.DTOs.SystemSetting;
using SGCM.Application.Interfaces;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Application.Services
{
    public class SystemSettingAppService : ISystemSettingAppService
    {
        private readonly ISystemSettingRepository _repository;
        private readonly IAuditLogDomainService _auditLogService;
        private readonly ILogger<SystemSettingAppService> _logger;

        public SystemSettingAppService(
            ISystemSettingRepository repository,
            IAuditLogDomainService auditLogService,
            ILogger<SystemSettingAppService> logger)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task<OperationResult<SystemSettingDto>> CreateAsync(AddSystemSettingDto createDto)
        {
            try
            {
                var existing = await _repository.GetByKeyAsync(createDto.SettingKey);
                if (existing.Data != null)
                    return OperationResult<SystemSettingDto>.Failure("A setting with this key already exists.");

                var setting = new SystemSetting
                {
                    SettingKey = createDto.SettingKey,
                    SettingValue = createDto.SettingValue,
                    Description = createDto.Description,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _repository.AddAsync(setting);
                if (!result.IsSuccess)
                    return OperationResult<SystemSettingDto>.Failure(result.Message);

                await _auditLogService.RecordCreateAsync(
                    userId: 1,
                    entityType: EntityType.SystemSetting,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("System setting created with key: {SettingKey}", result.Data.SettingKey);
                return OperationResult<SystemSettingDto>.Success(SystemSettingMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a system setting.");
                return OperationResult<SystemSettingDto>.Failure("An error occurred while creating the setting.");
            }
        }

        public async Task<OperationResult<SystemSettingDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<SystemSettingDto>.Failure(result.Message);

                return OperationResult<SystemSettingDto>.Success(SystemSettingMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching setting with ID {SettingId}.", id);
                return OperationResult<SystemSettingDto>.Failure("An error occurred while fetching the setting.");
            }
        }

        public async Task<OperationResult<List<SystemSettingDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                var settings = result.Data?.Select(SystemSettingMapper.ToResponse).ToList();
                return OperationResult<List<SystemSettingDto>>.Success(settings ?? new List<SystemSettingDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching system settings.");
                return OperationResult<List<SystemSettingDto>>.Failure("An error occurred while fetching settings.");
            }
        }

        public async Task<OperationResult<SystemSettingDto>> UpdateAsync(UpdateSystemSettingDto updateDto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(updateDto.Id);
                if (!existing.IsSuccess || existing.Data == null)
                    return OperationResult<SystemSettingDto>.Failure(existing.Message);

                var setting = existing.Data;
                setting.SettingValue = updateDto.SettingValue;
                setting.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(setting);
                if (!result.IsSuccess)
                    return OperationResult<SystemSettingDto>.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: 1,
                    entityType: EntityType.SystemSetting,
                    entityId: result.Data.Id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("System setting updated with ID: {SettingId}", result.Data.Id);
                return OperationResult<SystemSettingDto>.Success(SystemSettingMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating setting with ID {SettingId}.", updateDto.Id);
                return OperationResult<SystemSettingDto>.Failure("An error occurred while updating the setting.");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (!existing.IsSuccess || existing.Data == null)
                    return OperationResult.Failure(existing.Message);

                var result = await _repository.DeleteAsync(id);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                await _auditLogService.RecordDeleteAsync(
                    userId: 1,
                    entityType: EntityType.SystemSetting,
                    entityId: id,
                    previousEntity: existing.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("System setting deleted with ID: {SettingId}", id);
                return OperationResult.Success("Setting deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting setting with ID {SettingId}.", id);
                return OperationResult.Failure("An error occurred while deleting the setting.");
            }
        }

        public async Task<OperationResult<SystemSettingDto>> GetByKeyAsync(string key)
        {
            try
            {
                var result = await _repository.GetByKeyAsync(key);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<SystemSettingDto>.Failure("Setting not found.");

                return OperationResult<SystemSettingDto>.Success(SystemSettingMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching setting with key {SettingKey}.", key);
                return OperationResult<SystemSettingDto>.Failure("An error occurred while fetching the setting.");
            }
        }
    }
}