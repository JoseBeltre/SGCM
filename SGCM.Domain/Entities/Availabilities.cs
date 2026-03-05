using SGCM.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCM.Domain.Entities
{
    [Table("Availabilities")]
    public class Availabilities : UpdatableEntity
    {
        [Column("AvailabilityId")]
        [Key]
        public override int Id { get; set; }
        public int DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AppointmentDuration {  get; set; }
        public bool IsActive { get; set; } = true;
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }

    }
}
