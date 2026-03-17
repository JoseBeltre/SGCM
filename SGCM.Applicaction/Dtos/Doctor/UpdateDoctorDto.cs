namespace SGCM.Applicaction.DTOs.Doctor
{
    public record UpdateDoctorDto
    {
        public required int Id { get; init; }
        public required int SpecialtyId { get; init; }
        public string? AssignedOffice { get; init; }
    }
}