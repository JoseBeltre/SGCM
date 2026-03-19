using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface ISystemSettingRepository : IBaseRepository<SystemSetting>
    {
        Task<OperationResult<SystemSetting?>> GetByKeyAsync(string key);
    }
}