using System.Linq;
using System.Collections.Generic;

namespace SGCM.Desktop.Models
{
    /// <summary>
    /// Estado de sesión global para el usuario autenticado.
    /// </summary>
    public static class Session
    {
        public static string Token { get; set; } = "";
        public static int UserId { get; set; }
        public static string FullName { get; set; } = "";
        public static string Email { get; set; } = "";

        // Lista de roles del usuario actual
        public static List<string> Roles { get; set; } = new List<string>();

        public static string DisplayRole
        {
            get
            {
                if (Roles.Count == 0) return "Invitado";
                if (IsAdmin && IsDoctor) return "Admin / Doctor";
                return Roles[0];
            }
        }

        // Verificación robusta de roles (insensibles a mayúsculas)
        public static bool IsAdmin => Roles.Any(r => r.Equals("Admin", System.StringComparison.OrdinalIgnoreCase) || r.Equals("Administrador", System.StringComparison.OrdinalIgnoreCase));
        public static bool IsDoctor => Roles.Any(r => r.Equals("Doctor", System.StringComparison.OrdinalIgnoreCase) || r.Equals("Medico", System.StringComparison.OrdinalIgnoreCase));
        public static bool IsReceptionist => Roles.Any(r => r.Equals("Receptionist", System.StringComparison.OrdinalIgnoreCase) || r.Equals("Recepcionista", System.StringComparison.OrdinalIgnoreCase));

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Token) || UserId > 0;

        public static void Logout()
        {
            Token = "";
            Roles.Clear();
            UserId = 0;
            FullName = "";
            Email = "";
        }
    }
}
