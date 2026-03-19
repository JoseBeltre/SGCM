using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult<bool>> CanBeDeactivatedAsync(int userId);
        Task<OperationResult<bool>> DeactivateAsync(int userId);
    }
}