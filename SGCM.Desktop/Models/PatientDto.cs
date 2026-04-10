using System;

namespace SGCM.Desktop.Models
{
    /// <summary>
    /// Desktop model for Patient. Matches backend PatientDto fields.
    /// Note: Backend does NOT return IsActive, FullName, Email, Phone.
    /// Those are enriched client-side by joining with UserService.
    /// </summary>
    public class PatientDto
    {
        // --- Fields returned by backend API ---
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public SGCM.Domain.Enums.Gender? Gender { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? EmergencyContact { get; set; }
        public string? InsuranceNumber { get; set; }

        // --- Fields enriched client-side via UserService join ---
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true; // Default true since backend doesn't return it

        public string DisplayName => FullName ?? Email ?? "Paciente #" + Id;
    }
}
