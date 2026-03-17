using SGCM.Application.Base;
using SGCM.Application.DTOs.SystemSetting;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces
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