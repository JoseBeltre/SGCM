using SGCM.Domain.Base;

namespace SGCM.Domain.Entities
{
    public class Doctor : UpdatableEntity
    {
        public override int Id { get; set; }
        public int UserId { get; set; }
        public int SpecialtyId { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? AssignedOffice { get; set; }
        public bool IsActive { get; set; } = true;
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }
    }
}