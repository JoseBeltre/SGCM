namespace SGCM.Application.DTOs.Authentication
{
    public record AuthSessionDto
    {
        public required int Id { get; init; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required string UserType { get; init; }
        public string? NationalId { get; init; }
    }
}
