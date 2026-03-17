using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.User
{
    public record AddUserDto
    {
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public string? Phone { get; init; }
        public required string PasswordHash { get; init; }
        public required UserType UserType { get; init; }
    }
}