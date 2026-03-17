using SGCM.Application.DTOs.Availability;
using SGCM.Application.DTOs.Specialty;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class SpecialtyMapper
    {
        public static SpecialtyResponse ToResponse(Specialty specialty)
        {
            return new SpecialtyResponse
            {
                Id = specialty.Id,
                Name = specialty.Name,
                Description = specialty.Description,
                IsActive = specialty.IsActive
            };
        }
    }
}