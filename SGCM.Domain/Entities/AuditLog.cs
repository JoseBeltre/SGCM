using SGCM.Domain.Base;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public override int Id { get; set; }
        public int UserId { get; set; }
        public required EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public required AuditAction Action { get; set; }
        public string? PreviousValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
