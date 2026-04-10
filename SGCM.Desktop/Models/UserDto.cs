using System;

namespace SGCM.Desktop.Models
{
    /// <summary>
    /// Modelo para mostrar usuarios en las rejillas.
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public SGCM.Domain.Enums.UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public string DisplayName => $"{FullName} ({UserType})";
    }

    /// <summary>
    /// Sincronizado con AddUserDto de la API.
    /// </summary>
    public class UserCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Sincronizado con el nombre de campo en API
        public SGCM.Domain.Enums.UserType UserType { get; set; }
    }

    /// <summary>
    /// Sincronizado con UpdateUserDto de la API.
    /// </summary>
    public class UserUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
