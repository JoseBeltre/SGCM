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

        public AuthService(IUserRepository repository, IPatientRepository patientRepository)
        {
            _userRepository = repository;
            _patientRepository = patientRepository;
        }

        public async Task<OperationResult<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (!user.IsSuccess || user.Data == null)
                return OperationResult<UserDto>.Failure("Invalid credentials");

            if (!user.Data.IsActive)
                return OperationResult<UserDto>.Failure("User is inactive");

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Data.PasswordHash))
                return OperationResult<UserDto>.Failure("Invalid credentials");

            return OperationResult<UserDto>.Success(UserMapper.ToResponse(user.Data));
        }

        public async Task<OperationResult<UserDto>> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _userRepository.EmailExistsAsync(dto.Email);
            if (emailExists.Data)
                return OperationResult<UserDto>.Failure("Email already exists");

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
                return OperationResult<UserDto>.Failure(userResult.Message);

            var patient = new Patient
            {
                UserId = userResult.Data.Id,
                NationalId = dto.NationalId,
                DateOfBirth = dto.DateOfBirth,
                CreatedAt = DateTime.UtcNow
            };

            var patientResult = await _patientRepository.AddAsync(patient);

            if (!patientResult.IsSuccess)
                return OperationResult<UserDto>.Failure(patientResult.Message);

            return OperationResult<UserDto>.Success(UserMapper.ToResponse(userResult.Data));
        }
    }
}
