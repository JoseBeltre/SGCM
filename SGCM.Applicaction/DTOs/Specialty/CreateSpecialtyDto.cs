namespace SGCM.Application.DTOs.Specialty
{
    public record CreateSpecialtyDto
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
    }
}
