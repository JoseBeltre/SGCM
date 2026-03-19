using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Appointment
{
    public record AppointmentDto
    {
        public required int Id { get; init; }
        public required int PatientId { get; init; }
        public required int DoctorId { get; init; }
        public DateTime AppointmentDate { get; init; }
        public int DurationMinutes { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? ConsultationReason { get; init; }
        public string? DoctorNotes { get; init; }
        public string? CancellationReason { get; init; }
    }
}