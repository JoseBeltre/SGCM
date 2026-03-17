namespace SGCM.Applicaction.DTOs.Appointment
{
    public record AddAppointmentDto
    {
        public required int PatientId { get; init; }
        public required int DoctorId { get; init; }
        public required DateTime AppointmentDate { get; init; }
        public int DurationMinutes { get; init; } = 30;
        public string? ConsultationReason { get; init; }
    }
}