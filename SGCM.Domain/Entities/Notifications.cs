using SGCM.Domain.Base;
using SGCM.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCM.Domain.Entities
{
    [Table("Notifications")]
    public class Notifications : BaseEntity
    {
        [Column("NotificationId")]
        [Key]
        public override int Id { get; set; }
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public required string EventType { get; set; }
        public string? Message { get; set; }
        public string? Subject { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SendAttempts { get; set; }
        public string? ErrorDetail { get; set; }
    }
}
