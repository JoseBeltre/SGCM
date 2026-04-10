using System;

namespace SGCM.Desktop.Models
{
    public class UpdateDoctorDto
    {
        public int SpecialtyId { get; set; }
        public string? AssignedOffice { get; set; }
        public bool IsActive { get; set; }
    }
}
