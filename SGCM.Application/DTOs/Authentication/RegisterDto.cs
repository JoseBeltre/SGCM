namespace SGCM.Application.DTOs.Authentication
{
    public record RegisterDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public required string NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Phone { get; set; }
    }
}
