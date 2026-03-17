using Microsoft.Extensions.Logging;
using SGCM.Application.DTOs.User;
using SGCM.Application.Interfaces;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _repository;
        private readonly IUserService _domainService;
        private readonly IAuditLogDomainService _auditLogService;
        private readonly ILogger<UserAppService> _logger;

        public UserAppService(
            IUserRepository repository,
            IUserService domainService,
            IAuditLogDomainService auditLogService,
            ILogger<UserAppService> logger)
        {
            _repository = repository;
            _domainService = domainService;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task<OperationResult<UserDto>> CreateAsync(AddUserDto createDto)
        {
            try
            {
                var emailExists = await _repository.EmailExistsAsync(createDto.Email);
                if (emailExists.Data)
                    return OperationResult<UserDto>.Failure("A user with this email already exists.");

                var user = new User
                {
                    FullName = createDto.FullName,
                    Email = createDto.Email,
                    Phone = createDto.Phone,
                    PasswordHash = createDto.PasswordHash,
                    UserType = createDto.UserType,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _repository.AddAsync(user);
                if (!result.IsSuccess)
                    return OperationResult<UserDto>.Failure(result.Message);

                await _auditLogService.RecordCreateAsync(
                    userId: result.Data.Id,
                    entityType: EntityType.Patient,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("User created with ID: {UserId}", result.Data.Id);
                return OperationResult<UserDto>.Success(UserMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user.");
                return OperationResult<UserDto>.Failure("An error occurred while creating the user.");
            }
        }

        public async Task<OperationResult<UserDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<UserDto>.Failure(result.Message);

                return OperationResult<UserDto>.Success(UserMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with ID {UserId}.", id);
                return OperationResult<UserDto>.Failure("An error occurred while fetching the user.");
            }
        }

        public async Task<OperationResult<List<UserDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                var users = result.Data?.Select(UserMapper.ToResponse).ToList();
                return OperationResult<List<UserDto>>.Success(users ?? new List<UserDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return OperationResult<List<UserDto>>.Failure("An error occurred while fetching users.");
            }
        }

        public async Task<OperationResult<UserDto>> UpdateAsync(UpdateUserDto updateDto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(updateDto.Id);
                if (!existing.IsSuccess || existing.Data == null)
                    return OperationResult<UserDto>.Failure(existing.Message);

                var previous = existing.Data;
                previous.FullName = updateDto.FullName;
                previous.Email = updateDto.Email;
                previous.Phone = updateDto.Phone;
                previous.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(previous);
                if (!result.IsSuccess)
                    return OperationResult<UserDto>.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: result.Data.Id,
                    entityType: EntityType.Patient,
                    entityId: result.Data.Id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("User updated with ID: {UserId}", result.Data.Id);
                return OperationResult<UserDto>.Success(UserMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user with ID {UserId}.", updateDto.Id);
                return OperationResult<UserDto>.Failure("An error occurred while updating the user.");
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
                    userId: id,
                    entityType: EntityType.Patient,
                    entityId: id,
                    previousEntity: existing.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("User deleted with ID: {UserId}", id);
                return OperationResult.Success("User deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user with ID {UserId}.", id);
                return OperationResult.Failure("An error occurred while deleting the user.");
            }
        }

        public async Task<OperationResult> DeactivateAsync(int id)
        {
            try
            {
                var result = await _domainService.DeactivateAsync(id);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                _logger.LogInformation("User deactivated with ID: {UserId}", id);
                return OperationResult.Success("User deactivated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deactivating user with ID {UserId}.", id);
                return OperationResult.Failure("An error occurred while deactivating the user.");
            }
        }
    }
}