namespace SGCM.Application.DTOs.SystemSetting
{
    public record AddSystemSettingDto
    {
        public required string SettingKey { get; init; }
        public required string SettingValue { get; init; }
        public string? Description { get; init; }
    }
}