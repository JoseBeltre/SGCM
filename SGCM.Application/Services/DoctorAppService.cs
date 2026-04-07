using Microsoft.Extensions.Logging;
using SGCM.Application.DTOs.Doctor;
using SGCM.Application.Interfaces;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Application.Services
{
    public class DoctorAppService : IDoctorAppService
    {
        private readonly IDoctorRepository _repository;
        private readonly IDoctorService _domainService;
        private readonly IAuditLogDomainService _auditLogService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DoctorAppService> _logger;

        public DoctorAppService(
            IDoctorRepository repository,
            IDoctorService domainService,
            IAuditLogDomainService auditLogService,
            IUserRepository userRepository,
            ILogger<DoctorAppService> logger)
        {
            _repository = repository;
            _domainService = domainService;
            _auditLogService = auditLogService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<DoctorDto>> CreateAsync(AddDoctorDto createDto)
        {
            try
            {
                var existing = await _repository.GetByNationalIdAsync(createDto.NationalId);
                if (existing.Data != null)
                    return OperationResult<DoctorDto>.Failure("A doctor with this National ID already exists.");

                var doctor = new Doctor
                {
                    UserId = createDto.UserId,
                    SpecialtyId = createDto.SpecialtyId,
                    NationalId = createDto.NationalId,
                    LicenseNumber = createDto.LicenseNumber,
                    HireDate = createDto.HireDate,
                    AssignedOffice = createDto.AssignedOffice,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _repository.AddAsync(doctor);
                if (!result.IsSuccess)
                    return OperationResult<DoctorDto>.Failure(result.Message);

                await _auditLogService.RecordCreateAsync(
                    userId: result.Data.UserId,
                    entityType: EntityType.Doctor,
                    entityId: result.Data.Id,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Doctor created with ID: {DoctorId}", result.Data.Id);
                var userResult = await _userRepository.GetByIdAsync(result.Data.UserId);
                return OperationResult<DoctorDto>.Success(DoctorMapper.ToResponse(result.Data, userResult.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a doctor.");
                return OperationResult<DoctorDto>.Failure("An error occurred while creating the doctor.");
            }
        }

        public async Task<OperationResult<DoctorDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Data == null)
                    return OperationResult<DoctorDto>.Failure(result.Message);

                var userResult = await _userRepository.GetByIdAsync(result.Data.UserId);
                return OperationResult<DoctorDto>.Success(DoctorMapper.ToResponse(result.Data, userResult.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctor with ID {DoctorId}.", id);
                return OperationResult<DoctorDto>.Failure("An error occurred while fetching the doctor.");
            }
        }

        public async Task<OperationResult<List<DoctorDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                var doctors = result.Data ?? new List<Doctor>();
                var usersResult = await _userRepository.GetAllAsync();
                var users = usersResult.Data ?? new List<User>();
                
                var dtos = doctors.Select(d => DoctorMapper.ToResponse(d, users.FirstOrDefault(u => u.Id == d.UserId))).ToList();
                return OperationResult<List<DoctorDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctors.");
                return OperationResult<List<DoctorDto>>.Failure("An error occurred while fetching doctors.");
            }
        }

        public async Task<OperationResult<DoctorDto>> UpdateAsync(int id, UpdateDoctorDto updateDto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (!existing.IsSuccess || existing.Data == null)
                    return OperationResult<DoctorDto>.Failure(existing.Message);

                var doctor = existing.Data;
                doctor.SpecialtyId = updateDto.SpecialtyId;
                doctor.AssignedOffice = updateDto.AssignedOffice;
                doctor.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(doctor);
                if (!result.IsSuccess)
                    return OperationResult<DoctorDto>.Failure(result.Message);

                await _auditLogService.RecordUpdateAsync(
                    userId: result.Data.UserId,
                    entityType: EntityType.Doctor,
                    entityId: result.Data.Id,
                    previousEntity: existing.Data,
                    newEntity: result.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Doctor updated with ID: {DoctorId}", result.Data.Id);
                var userResult = await _userRepository.GetByIdAsync(result.Data.UserId);
                return OperationResult<DoctorDto>.Success(DoctorMapper.ToResponse(result.Data, userResult.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating doctor with ID {DoctorId}.", id);
                return OperationResult<DoctorDto>.Failure("An error occurred while updating the doctor.");
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
                    entityType: EntityType.Doctor,
                    entityId: id,
                    previousEntity: existing.Data,
                    ipAddress: "",
                    userAgent: "");

                _logger.LogInformation("Doctor deleted with ID: {DoctorId}", id);
                return OperationResult.Success("Doctor deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting doctor with ID {DoctorId}.", id);
                return OperationResult.Failure("An error occurred while deleting the doctor.");
            }
        }

        public async Task<OperationResult<List<DoctorDto>>> GetBySpecialtyIdAsync(int specialtyId)
        {
            try
            {
                var result = await _repository.GetDoctorsBySpecialtyIdAsync(specialtyId);
                var doctors = result.Data ?? new List<Doctor>();
                var usersResult = await _userRepository.GetAllAsync();
                var users = usersResult.Data ?? new List<User>();
                
                var dtos = doctors.Select(d => DoctorMapper.ToResponse(d, users.FirstOrDefault(u => u.Id == d.UserId))).ToList();
                return OperationResult<List<DoctorDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctors by specialty.");
                return OperationResult<List<DoctorDto>>.Failure("An error occurred while fetching doctors.");
            }
        }

        public async Task<OperationResult> DeactivateAsync(int id)
        {
            try
            {
                var result = await _domainService.DeactivateAsync(id);
                if (!result.IsSuccess)
                    return OperationResult.Failure(result.Message);

                _logger.LogInformation("Doctor deactivated with ID: {DoctorId}", id);
                return OperationResult.Success("Doctor deactivated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deactivating doctor with ID {DoctorId}.", id);
                return OperationResult.Failure("An error occurred while deactivating the doctor.");
            }
        }
    }
}