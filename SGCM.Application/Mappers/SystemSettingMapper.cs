using SGCM.Application.DTOs.SystemSetting;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class SystemSettingMapper
    {
        public static SystemSettingDto ToResponse(SystemSetting setting)
        {
            return new SystemSettingDto
            {
                Id = setting.Id,
                SettingKey = setting.SettingKey,
                SettingValue = setting.SettingValue,
                Description = setting.Description
            };
        }
    }
}