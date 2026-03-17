using SGCM.Domain.Enums;

namespace SGCM.Applicaction.DTOs.Availability
{
    public record UpdateAvailabilityExceptionDto
    {
        public required int Id { get; init; }
        public required int DoctorId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public string? Reason { get; init; }
        public ExceptionType ExceptionType { get; init; }
    }
}
