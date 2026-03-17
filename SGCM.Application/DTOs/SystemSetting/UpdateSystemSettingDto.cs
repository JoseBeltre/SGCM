namespace SGCM.Application.DTOs.SystemSetting
{
    public record UpdateSystemSettingDto
    {
        public required string SettingValue { get; init; }
    }
}