using SGCM.Domain.Enums;

namespace SGCM.Application.DTOs.Availability
{
    public record UpdateAvailabilityExceptionDto
    {
        public required int DoctorId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string? Reason { get; init; }
        public ExceptionType ExceptionType { get; init; }
    }
}
