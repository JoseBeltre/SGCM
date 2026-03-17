using SGCM.Applicaction.Base;
using SGCM.Applicaction.DTOs.User;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces
{
    public interface IUserAppService : IBaseService
        <IUserAppService,
        AddUserDto,
        UpdateUserDto,
        UserDto>
    {
        Task<OperationResult> DeactivateAsync(int id);
    }
}