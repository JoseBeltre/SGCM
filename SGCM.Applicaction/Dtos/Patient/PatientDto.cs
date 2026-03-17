using SGCM.Domain.Enums;

namespace SGCM.Applicaction.DTOs.Patient
{
    public record PatientDto
    {
        public required int Id { get; init; }
        public required int UserId { get; init; }
        public required string NationalId { get; init; }
        public DateTime DateOfBirth { get; init; }
        public string? Address { get; init; }
        public Gender? Gender { get; init; }
        public string? EmergencyPhone { get; init; }
        public string? EmergencyContact { get; init; }
        public string? InsuranceNumber { get; init; }
    }
}