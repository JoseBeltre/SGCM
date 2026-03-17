using Microsoft.Extensions.Logging;
using SGCM.Application.DTOs.Availability;
using SGCM.Application.Interfaces.Availability;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;

namespace SGCM.Application.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _repository;
        private readonly IAvailabilityExceptionRepository _exceptionsRepository;
        private readonly Domain.Services.Interfaces.IAvailabilityDomainService _domainService;
        private readonly ILogger<AvailabilityService> _logger;
        private readonly Domain.Services.Interfaces.IAuditLogDomainService _auditLogService;

        public AvailabilityService(
            IAvailabilityRepository availabilityRepository,
            IAvailabilityExceptionRepository availabilityExceptionRepository,
            Domain.Services.Interfaces.IAvailabilityDomainService domainService,
            ILogger<AvailabilityService> logger,
            Domain.Services.Interfaces.IAuditLogDomainService auditLogService)
        {
            _repository = availabilityRepository;
            _exceptionsRepository = availabilityExceptionRepository;
            _domainService = domainService;
            _logger = logger;
            _auditLogService = auditLogService;
        }

        public async Task<OperationResult<List<AvailabilityResponse>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();

                if (!result.IsSuccess)
                    return OperationResult<List<AvailabilityResponse>>.Failure(result.Message);

                var response = result.Data?
                    .Select(AvailabilityMapper.ToResponse)
                    .ToList();

                return OperationResult<List<AvailabilityResponse>>.Success(response ?? new List<AvailabilityResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching availabilities");
                return OperationResult<List<AvailabilityResponse>>.Failure("An error occurred while fetching availabilities.");
            }
        }

        public async Task<OperationResult<AvailabilityResponse>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);

                if (!result.IsSuccess)
                    return OperationResult<AvailabilityResponse>.Failure(result.Message);

                return OperationResult<AvailabilityResponse>.Success(
                    AvailabilityMapper.ToResponse(result.Data)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching availability with ID {AvailabilityId}", id);
                return OperationResult<AvailabilityResponse>.Failure("An error occurred while fetching the availability.");
            }
        }

        public async Task<OperationResult<AvailabilityResponse>> CreateAsync(CreateAvailabilityDto createDto)
        {
            try
            {
                if (createDto.StartTime >= createDto.EndTime)
                    return OperationResult<AvailabilityResponse>.Failure("Invalid time range.");

                var availability = new Domain.Entities.Availability
                {
                    DoctorId = createDto.DoctorId,
                    DayOfWeek = createDto.DayOfWeek,
                    StartTime = createDto.StartTime,
                    EndTime = createDto.EndTime,
                    IsActive = true
                };

                var result = await _repository.AddAsync(availability);

                if (!result.IsSuccess)
                    return OperationResult<AvailabilityResponse>.Failure(result.Message);

                _logger.LogInformation("Availability created with ID {AvailabilityId}", result.Data.Id);
                
                await _auditLogService.RecordCreateAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Availability,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                    );

                return OperationResult<AvailabilityResponse>.Success(
                    AvailabilityMapper.ToResponse(result.Data)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating availability");
                return OperationResult<AvailabilityResponse>.Failure("An error occurred while creating the availability.");
            }
        }

        public async Task<OperationResult<AvailabilityResponse>> UpdateAsync(int id, UpdateAvailabilityDto updateDto)
        {
            try
            {
                if (updateDto.StartTime >= updateDto.EndTime)
                    return OperationResult<AvailabilityResponse>.Failure("Invalid time range.");

                var existingResult = await _repository.GetByIdAsync(id);

                if (!existingResult.IsSuccess)
                    return OperationResult<AvailabilityResponse>.Failure(existingResult.Message);

                var availability = existingResult.Data;

                availability.StartTime = updateDto.StartTime;
                availability.EndTime = updateDto.EndTime;

                var result = await _repository.UpdateAsync(availability);

                if (!result.IsSuccess)
                    return OperationResult<AvailabilityResponse>.Failure(result.Message);

                _logger.LogInformation("Availability updated with ID {AvailabilityId}", availability.Id);

                await _auditLogService.RecordUpdateAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Availability,
                    entityId: availability.Id,
                    previousEntity: existingResult.Data,
                    newEntity: result.Data,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                );

                return OperationResult<AvailabilityResponse>.Success(
                    AvailabilityMapper.ToResponse(result.Data)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating availability with ID {AvailabilityId}", id);
                return OperationResult<AvailabilityResponse>.Failure("An error occurred while updating the availability.");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var existingResult = await _repository.GetByIdAsync(id);

                if (!existingResult.IsSuccess)
                    return OperationResult.Failure(existingResult.Message);

                var result = await _repository.DeleteAsync(id);

                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                _logger.LogInformation("Availability deleted with ID {AvailabilityId}", id);

                await _auditLogService.RecordDeleteAsync(
                    userId: 1, // UserId - replace with actual user ID from context
                    entityType: EntityType.Availability,
                    previousEntity: existingResult.Data,
                    entityId: id,
                    ipAddress: "", // IP Address - replace with actual IP address from context
                    userAgent: "" // User Agent - replace with actual user agent from context
                );

                return OperationResult.Success("Availability deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting availability with ID {AvailabilityId}", id);
                return OperationResult.Failure("An error occurred while deleting the availability.");
            }
        }

        public async Task<OperationResult<List<AvailabilityResponse>>> GetByDoctorIdAsync(int id)
        {
            try
            {
                var availabilities = await _repository.GetByDoctorIdAsync(id);

                if (!availabilities.IsSuccess)
                    return OperationResult<List<AvailabilityResponse>>.Failure(availabilities.Message);

                var response = availabilities.Data?
                    .Select(AvailabilityMapper.ToResponse)
                    .ToList();

                return OperationResult<List<AvailabilityResponse>>.Success(response ?? new List<AvailabilityResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching availabilities for doctor with ID {DoctorId}", id);
                return OperationResult<List<AvailabilityResponse>>.Failure("An error occurred while fetching availabilities.");
            }
        }
    }
}