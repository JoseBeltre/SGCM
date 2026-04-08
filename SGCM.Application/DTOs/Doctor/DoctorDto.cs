namespace SGCM.Application.DTOs.Doctor
{
    public record DoctorDto
    {
        public required int Id { get; init; }
        public required int UserId { get; init; }
        public required int SpecialtyId { get; init; }
        public required string NationalId { get; init; }
        public required string LicenseNumber { get; init; }
        public DateTime HireDate { get; init; }
        public string? AssignedOffice { get; init; }
        public bool IsActive { get; init; }

        public string? FullName { get; init; }
        public string? Email { get; init; }
        public string? Phone { get; init; }
    }
}
