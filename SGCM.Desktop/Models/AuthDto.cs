using System.Collections.Generic;

namespace SGCM.Desktop.Models
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Respuesta del endpoint de login sincronizada con AuthSessionDto de la API.
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        // Campos que coinciden exactamente con la API
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;

        /// <summary>
        /// Mapea el UserType de la API a la lista de roles del sistema Desktop.
        /// </summary>
        public List<string> GetEffectiveRoles()
        {
            var roles = new List<string>();
            if (!string.IsNullOrWhiteSpace(UserType))
            {
                roles.Add(UserType);
            }
            return roles;
        }
    }
}
