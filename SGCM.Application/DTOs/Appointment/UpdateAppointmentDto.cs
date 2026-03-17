namespace SGCM.Application.DTOs.Appointment
{
    public record UpdateAppointmentDto
    {
        public required DateTime AppointmentDate { get; init; }
        public int DurationMinutes { get; init; } = 30;
        public string? ConsultationReason { get; init; }
    }
}