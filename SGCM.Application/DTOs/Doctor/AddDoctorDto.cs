namespace SGCM.Application.DTOs.Doctor
{
    public record AddDoctorDto
    {
        public required int UserId { get; init; }
        public required int SpecialtyId { get; init; }
        public required string NationalId { get; init; }
        public required string LicenseNumber { get; init; }
        public required DateTime HireDate { get; init; }
        public string? AssignedOffice { get; init; }
    }
}