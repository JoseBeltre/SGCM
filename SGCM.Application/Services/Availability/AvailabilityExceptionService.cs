using Microsoft.Extensions.Logging;
using SGCM.Application.DTOs.Availability;
using SGCM.Application.Interfaces.Availability;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Application.Services
{
    public class AvailabilityExceptionService : IAvailabilityExceptionService
    {
        private readonly IAvailabilityExceptionRepository _repository;
        private readonly ILogger<AvailabilityExceptionService> _logger;
        private readonly IAuditLogDomainService _auditLogService;

        public AvailabilityExceptionService(
            IAvailabilityExceptionRepository availabilityExceptionRepository,
            IAuditLogDomainService auditLogService,
            ILogger<AvailabilityExceptionService> logger)
        {
            _repository = availabilityExceptionRepository;
            _logger = logger;
            _auditLogService = auditLogService;
        }

        public async Task<OperationResult<List<AvailabilityExceptionResponse>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();

                if (!result.IsSuccess)
                    return OperationResult<List<AvailabilityExceptionResponse>>.Failure(result.Message);

                var response = result.Data?
                    .Select(AvailabilityExceptionMapper.ToResponse)
                    .ToList();

                return OperationResult<List<AvailabilityExceptionResponse>>
                    .Success(response ?? new List<AvailabilityExceptionResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching availability exceptions");
                return OperationResult<List<AvailabilityExceptionResponse>>
                    .Failure("An error occurred while fetching availability exceptions.");
            }
        }

        public async Task<OperationResult<AvailabilityExceptionResponse>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);

                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<AvailabilityExceptionResponse>.Failure("AvailabilityException not found");

                return OperationResult<AvailabilityExceptionResponse>
                    .Success(AvailabilityExceptionMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching availability exception with ID {Id}", id);
                return OperationResult<AvailabilityExceptionResponse>
                    .Failure("An error occurred while fetching the availability exception.");
            }
        }

        public async Task<OperationResult<List<AvailabilityExceptionResponse>>> GetByDoctorIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByDoctorIdAsync(id);

                if (!result.IsSuccess)
                    return OperationResult<List<AvailabilityExceptionResponse>>.Failure(result.Message);

                var response = result.Data?
                    .Select(AvailabilityExceptionMapper.ToResponse)
                    .ToList();

                return OperationResult<List<AvailabilityExceptionResponse>>
                    .Success(response ?? new List<AvailabilityExceptionResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching availability exceptions for doctor {DoctorId}", id);
                return OperationResult<List<AvailabilityExceptionResponse>>
                    .Failure("An error occurred while fetching doctor availability exceptions.");
            }
        }

        public async Task<OperationResult<AvailabilityExceptionResponse>> CreateAsync(CreateAvailabilityExceptionDto createDto)
        {
            try
            {
                if (createDto.StartDate >= createDto.EndDate)
                    return OperationResult<AvailabilityExceptionResponse>.Failure("Invalid date range.");

                var conflict = await _repository.ExistsConflictAsync(
                    createDto.DoctorId,
                    createDto.StartDate,
                    createDto.EndDate);

                if (!conflict.IsSuccess)
                    return OperationResult<AvailabilityExceptionResponse>.Failure(conflict.Message);

                if (conflict.Data)
                    return OperationResult<AvailabilityExceptionResponse>.Failure("Availability exception conflict.");

                var entity = new AvailabilityException
                {
                    DoctorId = createDto.DoctorId,
                    StartDate = createDto.StartDate,
                    EndDate = createDto.EndDate,
                    Reason = createDto.Reason ?? "",
                    ExceptionType = createDto.ExceptionType
                };

                var result = await _repository.AddAsync(entity);

                if (!result.IsSuccess)
                    return OperationResult<AvailabilityExceptionResponse>.Failure(result.Message);

                _logger.LogInformation("AvailabilityException created with ID {Id}", result.Data.Id);

                await _auditLogService.RecordCreateAsync(
                    userId: 1,
                    entityType: EntityType.AvailabiltyException,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: ""
                );

                return OperationResult<AvailabilityExceptionResponse>
                    .Success(AvailabilityExceptionMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating availability exception");
                return OperationResult<AvailabilityExceptionResponse>
                    .Failure("An error occurred while creating the availability exception.");
            }
        }

        public async Task<OperationResult<AvailabilityExceptionResponse>> UpdateAsync(UpdateAvailabilityExceptionDto updateDto)
        {
            try
            {
                if (updateDto.StartDate >= updateDto.EndDate)
                    return OperationResult<AvailabilityExceptionResponse>.Failure("Invalid date range.");

                var existingResult = await _repository.GetByIdAsync(updateDto.Id);

                if (!existingResult.IsSuccess || existingResult.Data == null)
                    return OperationResult<AvailabilityExceptionResponse>.Failure("AvailabilityException not found");

                var entity = existingResult.Data;

                entity.DoctorId = updateDto.DoctorId;
                entity.StartDate = updateDto.StartDate;
                entity.EndDate = updateDto.EndDate;
                entity.Reason = updateDto.Reason ?? "";
                entity.ExceptionType = updateDto.ExceptionType;

                var result = await _repository.UpdateAsync(entity);

                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<AvailabilityExceptionResponse>.Failure(result.Message);

                _logger.LogInformation("AvailabilityException updated with ID {Id}", entity.Id);

                await _auditLogService.RecordUpdateAsync(
                    userId: 1,
                    entityType: EntityType.AvailabiltyException,
                    entityId: entity.Id,
                    previousEntity: existingResult.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: ""
                );

                return OperationResult<AvailabilityExceptionResponse>
                    .Success(AvailabilityExceptionMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating availability exception with ID {Id}", updateDto.Id);
                return OperationResult<AvailabilityExceptionResponse>
                    .Failure("An error occurred while updating the availability exception.");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);

                if (!existing.IsSuccess || existing.Data == null)
                    return OperationResult.Failure("AvailabilityException not found");

                var result = await _repository.DeleteAsync(id);

                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                _logger.LogInformation("AvailabilityException deleted with ID {Id}", id);

                await _auditLogService.RecordDeleteAsync(
                    userId: 1,
                    entityType: EntityType.AvailabiltyException,
                    previousEntity: existing.Data,
                    entityId: id,
                    ipAddress: "",
                    userAgent: ""
                );

                return OperationResult.Success("AvailabilityException deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting availability exception with ID {Id}", id);
                return OperationResult.Failure("An error occurred while deleting the availability exception.");
            }
        }
    }
}