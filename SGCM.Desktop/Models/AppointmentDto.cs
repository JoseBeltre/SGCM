using System;

namespace SGCM.Desktop.Models
{
    /// <summary>
    /// Matches the backend SGCM.Application.DTOs.Appointment.AppointmentDto exactly.
    /// Backend returns Status as string, not enum.
    /// </summary>
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ConsultationReason { get; set; }
        public string? DoctorNotes { get; set; }
        public string? CancellationReason { get; set; }
    }

    /// <summary>
    /// Matches backend AddAppointmentDto exactly.
    /// </summary>
    public class AppointmentCreateDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; } = 30;
        public string? ConsultationReason { get; set; }
    }
    
    /// <summary>
    /// Matches backend UpdateAppointmentDto exactly.
    /// </summary>
    public class AppointmentUpdateDto
    {
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; } = 30;
        public string? ConsultationReason { get; set; }
    }
}
