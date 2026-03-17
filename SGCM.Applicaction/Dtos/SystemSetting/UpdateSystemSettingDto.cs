namespace SGCM.Application.DTOs.SystemSetting
{
    public record UpdateSystemSettingDto
    {
        public required int Id { get; init; }
        public required string SettingValue { get; init; }
    }
}