using SGCM.Application.DTOs.Doctor;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorDto ToResponse(Doctor doctor, User? user = null)
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
                IsActive = doctor.IsActive,
                FullName = user?.FullName,
                Email = user?.Email,
                Phone = user?.Phone
            };
        }
    }
}