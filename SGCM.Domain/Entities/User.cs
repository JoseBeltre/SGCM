using SGCM.Domain.Base;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Entities
{
    public class User : UpdatableEntity
    {
        public override int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastAccess { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }
    }
}