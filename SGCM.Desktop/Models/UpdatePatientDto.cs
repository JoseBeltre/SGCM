using System;

namespace SGCM.Desktop.Models
{
    public class UpdatePatientDto
    {
        public string NationalId { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public SGCM.Domain.Enums.Gender? Gender { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? EmergencyContact { get; set; }
        public string? InsuranceNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
