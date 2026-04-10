using System;

namespace SGCM.Desktop.Models
{
    /// <summary>
    /// Desktop model for Doctor. Matches backend DoctorDto fields.
    /// Note: Backend returns AssignedOffice (not OfficeNumber), HireDate, IsActive.
    /// FullName, Email, Phone are enriched client-side by joining with UserService.
    /// </summary>
    public class DoctorDto
    {
        // --- Fields returned by backend API ---
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SpecialtyId { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? AssignedOffice { get; set; }
        public bool IsActive { get; set; }

        // --- Fields enriched client-side via UserService join ---
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string DisplayName => FullName ?? Email ?? "Doctor #" + Id;
    }
}
