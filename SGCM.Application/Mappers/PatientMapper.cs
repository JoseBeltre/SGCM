using SGCM.Application.DTOs.Patient;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class PatientMapper
    {
        public static PatientDto ToResponse(Patient patient, User? user = null)
        {
            return new PatientDto
            {
                Id = patient.Id,
                UserId = patient.UserId,
                NationalId = patient.NationalId,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address,
                Gender = patient.Gender,
                EmergencyPhone = patient.EmergencyPhone,
                EmergencyContact = patient.EmergencyContact,
                InsuranceNumber = patient.InsuranceNumber,
                FullName = user?.FullName,
                Email = user?.Email,
                Phone = user?.Phone
            };
        }
    }
}