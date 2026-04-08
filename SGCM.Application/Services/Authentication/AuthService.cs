using SGCM.Application.DTOs.Authentication;
using SGCM.Application.DTOs.User;
using SGCM.Application.Interfaces.Authentication;
using SGCM.Application.Mappers;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using BCrypt.Net;

namespace SGCM.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AuthService(IUserRepository repository, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _userRepository = repository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult<AuthSessionDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (!user.IsSuccess || user.Data == null)
                return OperationResult<AuthSessionDto>.Failure("Credenciales inválidas.");

            if (!user.Data.IsActive)
                return OperationResult<AuthSessionDto>.Failure("Este usuario se encuentra inactivo.");

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Data.PasswordHash))
                return OperationResult<AuthSessionDto>.Failure("Credenciales inválidas.");

            int profileId = 0;

            if (user.Data.UserType == UserType.Paciente)
            {
                var patient = await _patientRepository.GetByUserIdAsync(user.Data.Id);
                if (patient.IsSuccess && patient.Data != null)
                {
                    profileId = patient.Data.Id;
                }
            }
            else if (user.Data.UserType == UserType.Medico)
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(user.Data.Id);
                if (doctor.IsSuccess && doctor.Data != null)
                {
                    profileId = doctor.Data.Id;
                }
            }

            return OperationResult<AuthSessionDto>.Success(new AuthSessionDto
            {
                Id = user.Data.Id,
                ProfileId = profileId,
                FullName = user.Data.FullName,
                Email = user.Data.Email,
                UserType = user.Data.UserType.ToString()
            });
        }

        public async Task<OperationResult<AuthSessionDto>> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _userRepository.EmailExistsAsync(dto.Email);
            if (emailExists.Data)
                return OperationResult<AuthSessionDto>.Failure("El correo ya se encuentra registrado.");

            var patientExists = await _patientRepository.GetByNationalIdAsync(dto.NationalId);
            if (patientExists.IsSuccess && patientExists.Data != null)
                return OperationResult<AuthSessionDto>.Failure("El documento de identidad o cédula ya se encuentra registrado.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = passwordHash,
                UserType = UserType.Paciente,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastAccess = DateTime.UtcNow
            };

            var userResult = await _userRepository.AddAsync(user);

            if (!userResult.IsSuccess)
                return OperationResult<AuthSessionDto>.Failure(userResult.Message);

            var patient = new Patient
            {
                UserId = userResult.Data.Id,
                NationalId = dto.NationalId,
                DateOfBirth = dto.DateOfBirth,
                CreatedAt = DateTime.UtcNow
            };

            var patientResult = await _patientRepository.AddAsync(patient);

            if (!patientResult.IsSuccess)
                return OperationResult<AuthSessionDto>.Failure(patientResult.Message);

            return OperationResult<AuthSessionDto>.Success(new AuthSessionDto
            {
                Id = userResult.Data.Id,
                ProfileId = patientResult.Data.Id,
                FullName = userResult.Data.FullName,
                Email = userResult.Data.Email,
                UserType = userResult.Data.UserType.ToString()
            });
        }

        public async Task<OperationResult<AuthSessionDto>> GetSessionAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (!user.IsSuccess || user.Data == null || !user.Data.IsActive)
                return OperationResult<AuthSessionDto>.Failure("Session invalid or user inactive");

            int profileId = 0;

            if (user.Data.UserType == UserType.Paciente)
            {
                var patient = await _patientRepository.GetByUserIdAsync(user.Data.Id);
                if (patient.IsSuccess && patient.Data != null)
                {
                    profileId = patient.Data.Id;
                }
            }
            else if (user.Data.UserType == UserType.Medico)
            {
                var doctor = await _doctorRepository.GetByUserIdAsync(user.Data.Id);
                if (doctor.IsSuccess && doctor.Data != null)
                {
                    profileId = doctor.Data.Id;
                }
            }

            return OperationResult<AuthSessionDto>.Success(new AuthSessionDto
            {
                Id = user.Data.Id,
                ProfileId = profileId,
                FullName = user.Data.FullName,
                Email = user.Data.Email,
                UserType = user.Data.UserType.ToString()
            });
        }
    }
}
