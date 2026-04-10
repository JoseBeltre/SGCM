using System;

namespace SGCM.Desktop.Models
{
    /// <summary>
    /// Modelo para los logs de auditoria sincronizado con la entidad Domain.AuditLog.
    /// </summary>
    public class AuditLogDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public SGCM.Domain.Enums.EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public SGCM.Domain.Enums.AuditAction Action { get; set; }
        public string? PreviousValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime ActionDate { get; set; }

        // Propiedades calculadas para la UI
        public string DisplayAction => $"{Action} - {EntityType}";
        public string DisplayDetails => !string.IsNullOrEmpty(NewValues) ? NewValues : PreviousValues ?? "N/A";
    }
}
