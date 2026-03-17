using SGCM.Applicaction.Base;
using SGCM.Applicaction.DTOs.SystemSetting;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces
{
    public interface ISystemSettingAppService : IBaseService
        <ISystemSettingAppService,
        AddSystemSettingDto,
        UpdateSystemSettingDto,
        SystemSettingDto>
    {
        Task<OperationResult<SystemSettingDto>> GetByKeyAsync(string key);
    }
}