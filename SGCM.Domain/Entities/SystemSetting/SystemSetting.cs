using SGCM.Domain.Base;

namespace SGCM.Domain.Entities
{
    public class SystemSetting : UpdatableEntity
    {
        public override int Id { get; set; }
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string? Description { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }
    }
}