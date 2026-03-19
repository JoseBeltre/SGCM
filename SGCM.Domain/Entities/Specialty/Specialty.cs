using SGCM.Domain.Base;

namespace SGCM.Domain.Entities
{
    public class Specialty : UpdatableEntity
    {
        public override int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public override DateTime? UpdatedAt { get; set; }
        public override DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
