namespace SGCM.Application.DTOs.SystemSetting
{
    public record SystemSettingDto
    {
        public required int Id { get; init; }
        public required string SettingKey { get; init; }
        public required string SettingValue { get; init; }
        public string? Description { get; init; }
    }
}