namespace SGCM.Application.DTOs.Doctor
{
    public record UpdateDoctorDto
    {
        public required int SpecialtyId { get; init; }
        public string? AssignedOffice { get; init; }
    }
}