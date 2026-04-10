using Microsoft.AspNetCore.Mvc;
using SGCM.Application.Interfaces;
using SGCM.Domain.Repository;
using SGCM.Application.DTOs.User;

namespace SGCM.Api.Controllers
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAppService _userAppService;

        public AuthController(IUserRepository userRepository, IUserAppService userAppService)
        {
            _userRepository = userRepository;
            _userAppService = userAppService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Email y contraseña son requeridos.");

            var emailResult = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (!emailResult.IsSuccess || emailResult.Data == null)
            {
                // Fallback to searching by username if necessary, but we only have Email here
                return Unauthorized("Credenciales inválidas.");
            }

            var user = emailResult.Data;
            
            // PasswordHash actually stores the plain text string in our dummy implementation
            if (user.PasswordHash != loginDto.Password)
                return Unauthorized("Credenciales inválidas.");
                
            if (!user.IsActive)
                 return Unauthorized("El usuario está inactivo.");

            return Ok(new
            {
                Token = "dummy-token",
                Id = user.Id,
                ProfileId = user.Id, // dummy value mapping
                FullName = user.FullName,
                Email = user.Email,
                UserType = user.UserType.ToString()
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var addDto = new AddUserDto
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.Password,
                UserType = SGCM.Domain.Enums.UserType.Paciente // Default registration is patient
            };
            
            var result = await _userAppService.CreateAsync(addDto);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("session/{userId}")]
        public async Task<IActionResult> GetSession(int userId)
        {
            var userResult = await _userAppService.GetByIdAsync(userId);
            if (!userResult.IsSuccess || userResult.Data == null)
                return Unauthorized(userResult.Message);

            var user = userResult.Data;
            return Ok(new
            {
                Token = "dummy-token",
                Id = user.Id,
                ProfileId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserType = user.UserType.ToString()
            });
        }
    }
}
