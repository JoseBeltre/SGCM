using SGCM.Application.DTOs.Appointment;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class AppointmentMapper
    {
        public static AppointmentDto ToResponse(Appointment appointment)
        {
            return new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                DurationMinutes = appointment.DurationMinutes,
                Status = appointment.Status,
                ConsultationReason = appointment.ConsultationReason,
                DoctorNotes = appointment.DoctorNotes,
                CancellationReason = appointment.CancellationReason
            };
        }
    }
}