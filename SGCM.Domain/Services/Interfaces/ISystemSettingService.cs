using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface ISystemSettingService
    {
        Task<OperationResult<bool>> KeyExistsAsync(string key);
    }
}