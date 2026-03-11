using SGCM.Domain.Base;

namespace SGCM.Domain.Entities
{
    public class AppointmentHistory : BaseEntity
    {
        public override int Id { get; set; }
        public int AppointmentId { get; set; }
        public string? PreviousStatus { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public DateTime? PreviousDate { get; set; }
        public DateTime? NewDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public string? Notes { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}