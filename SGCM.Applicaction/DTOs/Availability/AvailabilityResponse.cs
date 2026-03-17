using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Availability
{
    public record AvailabilityResponse
    {
        public required int Id { get; init; }
        public required int DoctorId { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public Domain.Enums.DayOfWeek DayOfWeek { get; init; }
        public bool IsActive { get; init; }
        public int AppointmentDuration { get; init; }
    }
}
