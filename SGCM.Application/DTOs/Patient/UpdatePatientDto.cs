using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Patient
{
    public record UpdatePatientDto
    {
        public required int Id { get; init; }
        public string? Address { get; init; }
        public Gender? Gender { get; init; }
        public string? EmergencyPhone { get; init; }
        public string? EmergencyContact { get; init; }
        public string? InsuranceNumber { get; init; }
    }
}