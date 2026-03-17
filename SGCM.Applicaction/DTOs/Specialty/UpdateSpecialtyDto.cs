namespace SGCM.Application.DTOs.Specialty
{
    public record UpdateSpecialtyDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
    }
}
