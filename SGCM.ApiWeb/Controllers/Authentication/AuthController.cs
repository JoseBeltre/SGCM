using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SGCM.Application.DTOs.Authentication;
using SGCM.Application.Interfaces.Authentication;
using System.Security.Claims;

namespace SGCM.ApiWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                new Claim(ClaimTypes.Name, result.Data.FullName),
                new Claim(ClaimTypes.Role, result.Data.UserType)
            }, "Cookies")));

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.IsSuccess)
                return Unauthorized(result.Message);

            var session = result.Data;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, session.Id.ToString()),
                new Claim(ClaimTypes.Name, session.FullName),
                new Claim(ClaimTypes.Role, session.UserType)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return Ok(session);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetSession()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var result = await _authService.GetSessionAsync(userId);
            if (!result.IsSuccess)
                return Unauthorized();

            return Ok(result.Data);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return NoContent();
        }
    }
}
