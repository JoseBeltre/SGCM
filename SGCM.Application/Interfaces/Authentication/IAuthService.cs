using SGCM.Application.DTOs.Authentication;
using SGCM.Application.DTOs.User;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<OperationResult<AuthSessionDto>> RegisterAsync(RegisterDto registerDto);
        Task<OperationResult<AuthSessionDto>> LoginAsync(LoginDto loginDto);
        Task<OperationResult<AuthSessionDto>> GetSessionAsync(int userId);
    }
}
