namespace SGCM.Application.DTOs.User
{
    public record UpdateUserDto
    {
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public string? Phone { get; init; }
    }
}