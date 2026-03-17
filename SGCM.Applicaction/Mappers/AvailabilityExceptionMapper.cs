using SGCM.Application.DTOs.Availability;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class AvailabilityExceptionMapper
    {
        public static AvailabilityExceptionResponse ToResponse(AvailabilityException availabilityException)
        {
            return new AvailabilityExceptionResponse
            {
                Id = availabilityException.Id,
                DoctorId = availabilityException.DoctorId,
                StartDate = availabilityException.StartDate,
                EndDate = availabilityException.EndDate,
                Reason = availabilityException.Reason,
                ExceptionType = availabilityException.ExceptionType
            };
        }
    }
}
