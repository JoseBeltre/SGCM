using SGCM.Domain.Base;
using SGCM.Application.DTOs.Specialty;
using Microsoft.Extensions.Logging;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;
using SGCM.Application.Mappers;
using SGCM.Application.Interfaces.Specialty;
namespace SGCM.Application.Services.Specialty
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ISpecialtyRepository _repository;
        private readonly ILogger<SpecialtyService> _logger;
        private readonly IAuditLogDomainService _auditlogService;
        public SpecialtyService(ISpecialtyRepository specialtiesRepository, ILogger<SpecialtyService> logger, IAuditLogDomainService auditLogService)
        {
            _repository = specialtiesRepository;
            _logger = logger;
            _auditlogService = auditLogService;
        }

        public async Task<OperationResult<List<SpecialtyResponse>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                var specialties = result.Data?.Select(s => new SpecialtyResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    IsActive = s.IsActive
                }).ToList();

                return OperationResult<List<SpecialtyResponse>>.Success(specialties ?? new List<SpecialtyResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching specialties.");
                return OperationResult<List<SpecialtyResponse>>.Failure("An error occurred while fetching specialties.");
            }
        }

        public async Task<OperationResult<SpecialtyResponse>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.IsSuccess)
                    return OperationResult<SpecialtyResponse>.Failure(result.Message);

                return OperationResult<SpecialtyResponse>.Success(SpecialtyMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching a specialty.");
                return OperationResult<SpecialtyResponse>.Failure("An error occurred while fetching the specialty.");
            }
        }

        public async Task<OperationResult<SpecialtyResponse>> CreateAsync(CreateSpecialtyDto request)
        {
            try
            {
                var specialty = new Domain.Entities.Specialty
                {
                    Name = request.Name,
                    Description = request.Description
                };

                var result = await _repository.AddAsync(specialty);

                if (!result.IsSuccess)
                    return OperationResult<SpecialtyResponse>.Failure(result.Message);

                await _auditlogService.RecordCreateAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Specialty,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                    );

                _logger.LogInformation("Specialty created with ID: {SpecialtyId}", result.Data);
                return OperationResult<SpecialtyResponse>.Success(SpecialtyMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a specialty.");
                return OperationResult<SpecialtyResponse>.Failure("An error occurred while creating the specialty.");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var existingSpecialtyResult = await _repository.GetByIdAsync(id);
                if (!existingSpecialtyResult.IsSuccess)
                    return OperationResult.Failure(existingSpecialtyResult.Message);

                var result = await _repository.DeleteAsync(id);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                await _auditlogService.RecordDeleteAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Specialty,
                    previousEntity: existingSpecialtyResult.Data,
                    entityId: id,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                    );

                _logger.LogInformation("Specialty deleted with ID: {SpecialtyId}", id);
                return OperationResult.Success("Specialty deleted successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a specialty.");
                return OperationResult.Failure("An error occurred while deleting the specialty.");
            }
        }

        public async Task<OperationResult<SpecialtyResponse>> UpdateAsync(UpdateSpecialtyDto request)
        {
            try
            {
                var existingSpecialtyResult = await _repository.GetByIdAsync(request.Id);
                if (!existingSpecialtyResult.IsSuccess)
                    return OperationResult<SpecialtyResponse>.Failure(existingSpecialtyResult.Message);

                var specialty = existingSpecialtyResult.Data;
                specialty.Name = request.Name;
                specialty.Description = request.Description;

                var result = await _repository.UpdateAsync(specialty);

                if (!result.IsSuccess)
                    return OperationResult<SpecialtyResponse>.Failure(result.Message);

                await _auditlogService.RecordUpdateAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Specialty,
                    previousEntity: existingSpecialtyResult.Data,
                    newEntity: result.Data,
                    entityId: request.Id,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                );
                _logger.LogInformation("Specialty updated with ID: {SpecialtyId}", request.Id);
                return OperationResult<SpecialtyResponse>.Success(SpecialtyMapper.ToResponse(specialty));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a specialty.");
                return OperationResult<SpecialtyResponse>.Failure("An error occurred while updating the specialty.");
            }
        }
        public async Task<OperationResult<List<SpecialtyResponse>>> GetActiveAsync()
        {
            try
            {
                var result = await _repository.GetActiveAsync();
                var specialties = result.Data?.Select(SpecialtyMapper.ToResponse).ToList();
                return OperationResult<List<SpecialtyResponse>>.Success(specialties ?? new List<SpecialtyResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching active specialties.");
                return OperationResult<List<SpecialtyResponse>>.Failure("An error occurred while fetching active specialties.");
            }
        }

        public async Task<OperationResult> DeactivateAsync(int id)
        {
            try
            {
                var result = await _repository.DeactivateAsync(id);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                await _auditlogService.RecordUpdateAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Specialty,
                    previousEntity: "",
                    newEntity: "",
                    entityId: id,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                );
                _logger.LogInformation("Specialty deactivated with ID: {SpecialtyId}", id);
                return OperationResult.Success("Specialty deactivated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deactivating a specialty.");
                return OperationResult.Failure("An error occurred while deactivating the specialty.");
            }
        }
    }
}
