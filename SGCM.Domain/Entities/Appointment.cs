using SGCM.Domain.Base;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Entities
{
    public class Appointment : UpdatableEntity
    {
        public override int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; } = 30;
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Solicitada;
        public string? ConsultationReason { get; set; }
        public string? DoctorNotes { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }
    }
}