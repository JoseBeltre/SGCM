using SGCM.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGCM.Domain.Entities
{
    [Table("Specialties")]
    public class Specialties : UpdatableEntity
    {
        [Column("SpecialtyId")]
        [Key]
        public override int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public override DateTime? UpdatedAt { get; set; }
        public override DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
