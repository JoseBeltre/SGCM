using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.User
{
    public record UserDto
    {
        public required int Id { get; init; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public string? Phone { get; init; }
        public string UserType { get; init; } = Domain.Enums.UserType.Paciente.ToString();
        public bool IsActive { get; init; }
        public DateTime? LastAccess { get; init; }
    }
}