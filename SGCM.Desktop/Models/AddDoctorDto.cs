using System;

namespace SGCM.Desktop.Models
{
    public class AddDoctorDto
    {
        public int UserId { get; set; }
        public int SpecialtyId { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? AssignedOffice { get; set; } = string.Empty;
    }
}
