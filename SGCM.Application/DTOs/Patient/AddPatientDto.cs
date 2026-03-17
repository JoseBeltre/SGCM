using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Patient
{
    public record AddPatientDto
    {
        public required int UserId { get; init; }
        public required string NationalId { get; init; }
        public required DateTime DateOfBirth { get; init; }
        public string? Address { get; init; }
        public Gender? Gender { get; init; }
        public string? EmergencyPhone { get; init; }
        public string? EmergencyContact { get; init; }
        public string? InsuranceNumber { get; init; }
    }
}