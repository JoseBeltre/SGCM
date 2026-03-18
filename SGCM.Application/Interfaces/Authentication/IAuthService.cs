using SGCM.Application.DTOs.Authentication;
using SGCM.Application.DTOs.User;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<OperationResult<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<OperationResult<UserDto>> LoginAsync(LoginDto loginDto);
    }
}
