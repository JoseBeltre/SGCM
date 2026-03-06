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
    public class AuditLogs : BaseEntity
    {
        public override int Id { get; set; }
        public int UserId { get; set; }
        public required EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public required Action Action { get; set; }
        public string? PreviousValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
