using Microsoft.Extensions.Logging;
using SGCM.Applicaction.DTOs.Patient;
using SGCM.Applicaction.Interfaces;
using SGCM.Applicaction.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Applicaction.Services
{
    public class PatientAppService : IPatientAppService
    {
        private readonly IPatientRepository _repository;
        private readonly IAuditLogDomainService _auditLogService;
        private readonly ILogger<PatientAppService> _logger;

        public PatientAppService(
            IPatientRepository repository,
            IAuditLogDomainService auditLogService,
            ILogger<PatientAppService> logger)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public async Task<OperationResult<PatientDto>> CreateAsync(AddPatientDto createDto)
        {
            try
            {
                var existing = await _repository.GetByNationalIdAsync(createDto.NationalId);
                if (existing.Data != null)
                    return OperationResult<PatientDto>.Failure("A patient with this National ID already exists.");

                var patient = new Patient
                {
                    UserId = createDto.UserId,
                    NationalId = createDto.NationalId,
                    DateOfBirth = createDto.DateOfBirth,
                    Address = createDto.Address,
                    Gender = createDto.Gender,
                    EmergencyPhone = createDto.EmergencyPhone,
                    EmergencyContact = createDto.EmergencyContact,
                    InsuranceNumber = createDto.InsuranceNumber,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _repository.AddAsync(patient);
                if (!result.IsSuccess)
                    return OperationResult<PatientDto>.Failure(result.Message);

                await _auditLogService.RecordCreateAsync(
                    userId: result.Data.UserId,
                    entityType: EntityType.Patient,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Patient created with ID: {PatientId}", result.Data.Id);
                return OperationResult<PatientDto>.Success(PatientMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a patient.");
                return OperationResult<PatientDto>.Failure("An error occurred while creating the patient.");
            }
        }

        public async Task<OperationResult<PatientDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<PatientDto>.Failure(result.Message);

                return OperationResult<PatientDto>.Success(PatientMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching patient with ID {PatientId}.", id);
                return OperationResult<PatientDto>.Failure("An error occurred while fetching the patient.");
            }
        }

        public async Task<OperationResult<List<PatientDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                var patients = result.Data?.Select(PatientMapper.ToResponse).ToList();
                return OperationResult<List<PatientDto>>.Success(patients ?? new List<PatientDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching patients.");
                return OperationResult<List<PatientDto>>.Failure("An error occurred while fetching patients.");
            }
        }

        public async Task<OperationResult<PatientDto>> UpdateAsync(UpdatePatientDto updateDto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(updateDto.Id);
                if (!existing.IsSuccess || existing.Data == null)
                    return OperationResult<PatientDto>.Failure(existing.Message);

                var patient = existing.Data;
                patient.Address = updateDto.Address;
                patient.Gender = updateDto.Gender;
                patient.EmergencyPhone = updateDto.EmergencyPhone;
                patient.EmergencyContact = updateDto.EmergencyContact;
                patient.InsuranceNumber = updateDto.InsuranceNumber;
                patient.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(patient);
                if (!result.IsSuccess)
                    return OperationResult<PatientDto>.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: result.Data.UserId,
                    entityType: EntityType.Patient,
                    entityId: result.Data.Id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Patient updated with ID: {PatientId}", result.Data.Id);
                return OperationResult<PatientDto>.Success(PatientMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating patient with ID {PatientId}.", updateDto.Id);
                return OperationResult<PatientDto>.Failure("An error occurred while updating the patient.");
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
                    userId: existing.Data.UserId,
                    entityType: EntityType.Patient,
                    entityId: id,
                    previousEntity: existing.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Patient deleted with ID: {PatientId}", id);
                return OperationResult.Success("Patient deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting patient with ID {PatientId}.", id);
                return OperationResult.Failure("An error occurred while deleting the patient.");
            }
        }

        public async Task<OperationResult<PatientDto>> GetByNationalIdAsync(string nationalId)
        {
            try
            {
                var result = await _repository.GetByNationalIdAsync(nationalId);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<PatientDto>.Failure("Patient not found.");

                return OperationResult<PatientDto>.Success(PatientMapper.ToResponse(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching patient by National ID.");
                return OperationResult<PatientDto>.Failure("An error occurred while fetching the patient.");
            }
        }
    }
}