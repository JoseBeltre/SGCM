using SGCM.Application.Base;
using SGCM.Application.DTOs.User;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces
{
    public interface IUserAppService : IBaseService
        <IUserAppService,
        AddUserDto,
        UpdateUserDto,
        UserDto>
    {
        Task<OperationResult<UserDto>> UpdateAsync(int id, UpdateUserDto updateDto);
        Task<OperationResult> DeactivateAsync(int id);
    }
}