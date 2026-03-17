using SGCM.Applicaction.DTOs.Patient;
using SGCM.Domain.Entities;

namespace SGCM.Applicaction.Mappers
{
    public static class PatientMapper
    {
        public static PatientDto ToResponse(Patient patient)
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
                InsuranceNumber = patient.InsuranceNumber
            };
        }
    }
}