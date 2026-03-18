using SGCM.Application.DTOs.Availability;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class AvailabilityMapper
    {
        public static AvailabilityResponse ToResponse(Availability availability)
        {
            return new AvailabilityResponse
            {
                Id = availability.Id,
                DoctorId = availability.DoctorId,
                DayOfWeek = availability.DayOfWeek.ToString(),
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                IsActive = availability.IsActive
            };
        }
    }
}