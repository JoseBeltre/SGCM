using SGCM.Domain.Base;

namespace SGCM.Domain.Entities
{
    public class Availability : UpdatableEntity
    {
        public override int Id { get; set; }
        public int DoctorId { get; set; }
        public Domain.Enums.DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AppointmentDuration {  get; set; }
        public bool IsActive { get; set; } = true;
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }

    }
}
