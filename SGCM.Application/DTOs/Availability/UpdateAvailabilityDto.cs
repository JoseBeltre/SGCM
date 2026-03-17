using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Availability
{
    public record UpdateAvailabilityDto
    {
        public required int Id { get; init; }
        public int DoctorId { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public Domain.Enums.DayOfWeek DayOfWeek { get; init; }
        public int? AppointmentDuration { get; init; }
    }
}
