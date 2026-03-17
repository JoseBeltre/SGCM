using Microsoft.Extensions.Logging;
using SGCM.Application.DTOs.Appointment;
using SGCM.Application.Interfaces;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Application.Services
{
    public class AppointmentAppService : IAppointmentAppService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IAppointmentService _domainService;
        private readonly IAuditLogDomainService _auditLogService;
        private readonly ILogger<AppointmentAppService> _logger;

        public AppointmentAppService(
            IAppointmentRepository repository,
            IAppointmentService domainService,
            IAuditLogDomainService auditLogService,
            ILogger<AppointmentAppService> logger)
        {
            _repository = repository;
            _domainService = domainService;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task<OperationResult<AppointmentDto>> CreateAsync(AddAppointmentDto createDto)
        {
            try
            {
                var hasConflict = await _domainService.HasSchedulingConflictAsync(
                    createDto.DoctorId, createDto.AppointmentDate, createDto.DurationMinutes);
                if (!hasConflict.IsSuccess)
                    return OperationResult<AppointmentDto>.Failure(hasConflict.Message);
                if (hasConflict.Data)
                    return OperationResult<AppointmentDto>.Failure("The doctor already has an appointment at this time.");

                var appointment = new Appointment
                {
                    PatientId = createDto.PatientId,
                    DoctorId = createDto.DoctorId,
                    AppointmentDate = createDto.AppointmentDate,
                    DurationMinutes = createDto.DurationMinutes,
                    ConsultationReason = createDto.ConsultationReason,
                    Status = AppointmentStatus.Solicitada,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _repository.AddAsync(appointment);
                if (!result.IsSuccess)
                    return OperationResult<AppointmentDto>.Failure(result.Message);

                await _auditLogService.RecordCreateAsync(
                    userId: result.Data.PatientId,
                    entityType: EntityType.Appointment,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Appointment created with ID: {AppointmentId}", result.Data.Id);
                return OperationResult<AppointmentDto>.Success(AppointmentMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an appointment.");
                return OperationResult<AppointmentDto>.Failure("An error occurred while creating the appointment.");
            }
        }

        public async Task<OperationResult<AppointmentDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<AppointmentDto>.Failure(result.Message);

                return OperationResult<AppointmentDto>.Success(AppointmentMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching appointment with ID {AppointmentId}.", id);
                return OperationResult<AppointmentDto>.Failure("An error occurred while fetching the appointment.");
            }
        }

        public async Task<OperationResult<List<AppointmentDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                var appointments = result.Data?.Select(AppointmentMapper.ToResponse).ToList();
                return OperationResult<List<AppointmentDto>>.Success(appointments ?? new List<AppointmentDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching appointments.");
                return OperationResult<List<AppointmentDto>>.Failure("An error occurred while fetching appointments.");
            }
        }

        public async Task<OperationResult<AppointmentDto>> UpdateAsync(int id, UpdateAppointmentDto updateDto)
        {
            try
            {
                var canReschedule = await _domainService.CanBeRescheduledAsync(
                    updateDto.Id, updateDto.AppointmentDate);
                if (!canReschedule.IsSuccess || !canReschedule.Data)
                    return OperationResult<AppointmentDto>.Failure(canReschedule.Message);

                var existing = await _repository.GetByIdAsync(id);
                if (!existing.IsSuccess || existing.Data == null)
                    return OperationResult<AppointmentDto>.Failure(existing.Message);

                var appointment = existing.Data;
                appointment.AppointmentDate = updateDto.AppointmentDate;
                appointment.DurationMinutes = updateDto.DurationMinutes;
                appointment.ConsultationReason = updateDto.ConsultationReason;
                appointment.Status = AppointmentStatus.Solicitada;
                appointment.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(appointment);
                if (!result.IsSuccess)
                    return OperationResult<AppointmentDto>.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: result.Data.PatientId,
                    entityType: EntityType.Appointment,
                    entityId: result.Data.Id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Appointment rescheduled with ID: {AppointmentId}", result.Data.Id);
                return OperationResult<AppointmentDto>.Success(AppointmentMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating appointment with ID {AppointmentId}.", id);
                return OperationResult<AppointmentDto>.Failure("An error occurred while updating the appointment.");
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
                    userId: existing.Data.PatientId,
                    entityType: EntityType.Appointment,
                    entityId: id,
                    previousEntity: existing.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Appointment deleted with ID: {AppointmentId}", id);
                return OperationResult.Success("Appointment deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting appointment with ID {AppointmentId}.", id);
                return OperationResult.Failure("An error occurred while deleting the appointment.");
            }
        }

        public async Task<OperationResult<List<AppointmentDto>>> GetByPatientIdAsync(int patientId)
        {
            try
            {
                var result = await _repository.GetByPatientIdAsync(patientId);
                var appointments = result.Data?.Select(AppointmentMapper.ToResponse).ToList();
                return OperationResult<List<AppointmentDto>>.Success(appointments ?? new List<AppointmentDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching appointments for patient {PatientId}.", patientId);
                return OperationResult<List<AppointmentDto>>.Failure("An error occurred while fetching appointments.");
            }
        }

        public async Task<OperationResult<List<AppointmentDto>>> GetByDoctorIdAsync(int doctorId)
        {
            try
            {
                var result = await _repository.GetByDoctorIdAsync(doctorId);
                var appointments = result.Data?.Select(AppointmentMapper.ToResponse).ToList();
                return OperationResult<List<AppointmentDto>>.Success(appointments ?? new List<AppointmentDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching appointments for doctor {DoctorId}.", doctorId);
                return OperationResult<List<AppointmentDto>>.Failure("An error occurred while fetching appointments.");
            }
        }

        public async Task<OperationResult> ConfirmAsync(int id)
        {
            try
            {
                var canConfirm = await _domainService.CanBeConfirmedAsync(id);
                if (!canConfirm.IsSuccess || !canConfirm.Data)
                    return OperationResult.Failure(canConfirm.Message);

                var existing = await _repository.GetByIdAsync(id);
                existing.Data!.Status = AppointmentStatus.Confirmada;
                existing.Data.ConfirmedAt = DateTime.UtcNow;
                existing.Data.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(existing.Data);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: existing.Data.DoctorId,
                    entityType: EntityType.Appointment,
                    entityId: id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Appointment confirmed with ID: {AppointmentId}", id);
                return OperationResult.Success("Appointment confirmed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while confirming appointment with ID {AppointmentId}.", id);
                return OperationResult.Failure("An error occurred while confirming the appointment.");
            }
        }

        public async Task<OperationResult> CancelAsync(int id, string reason)
        {
            try
            {
                var canCancel = await _domainService.CanBeCancelledAsync(id);
                if (!canCancel.IsSuccess || !canCancel.Data)
                    return OperationResult.Failure(canCancel.Message);

                var existing = await _repository.GetByIdAsync(id);
                existing.Data!.Status = AppointmentStatus.Cancelada;
                existing.Data.CancellationReason = reason;
                existing.Data.CancelledAt = DateTime.UtcNow;
                existing.Data.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(existing.Data);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: existing.Data.PatientId,
                    entityType: EntityType.Appointment,
                    entityId: id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Appointment cancelled with ID: {AppointmentId}", id);
                return OperationResult.Success("Appointment cancelled successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cancelling appointment with ID {AppointmentId}.", id);
                return OperationResult.Failure("An error occurred while cancelling the appointment.");
            }
        }

        public async Task<OperationResult> CompleteAsync(int id)
        {
            try
            {
                var canComplete = await _domainService.CanBeCompletedAsync(id);
                if (!canComplete.IsSuccess || !canComplete.Data)
                    return OperationResult.Failure(canComplete.Message);

                var existing = await _repository.GetByIdAsync(id);
                existing.Data!.Status = AppointmentStatus.Completada;
                existing.Data.CompletedAt = DateTime.UtcNow;
                existing.Data.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(existing.Data);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: existing.Data.DoctorId,
                    entityType: EntityType.Appointment,
                    entityId: id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Appointment completed with ID: {AppointmentId}", id);
                return OperationResult.Success("Appointment completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while completing appointment with ID {AppointmentId}.", id);
                return OperationResult.Failure("An error occurred while completing the appointment.");
            }
        }
    }
}