using SGCM.Domain.Base;
using SGCM.Domain.Enums;
using System.Reflection;

namespace SGCM.Domain.Entities
{
    public class Patient : UpdatableEntity
    {
        public override int Id { get; set; }
        public int UserId { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? EmergencyContact { get; set; }
        public string? InsuranceNumber { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }
    }
}