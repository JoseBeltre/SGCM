using SGCM.Applicaction.DTOs.Doctor;
using SGCM.Domain.Entities;

namespace SGCM.Applicaction.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorDto ToResponse(Doctor doctor)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                UserId = doctor.UserId,
                SpecialtyId = doctor.SpecialtyId,
                NationalId = doctor.NationalId,
                LicenseNumber = doctor.LicenseNumber,
                HireDate = doctor.HireDate,
                AssignedOffice = doctor.AssignedOffice,
                IsActive = doctor.IsActive
            };
        }
    }
}