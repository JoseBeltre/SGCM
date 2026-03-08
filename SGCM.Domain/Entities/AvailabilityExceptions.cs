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
    [Table("AvailabilityExceptions")]
    public class AvailabilityExceptions : BaseEntity
    {
        [Column("ExceptionId")]
        [Key]
        public override int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public ExceptionType ExceptionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
