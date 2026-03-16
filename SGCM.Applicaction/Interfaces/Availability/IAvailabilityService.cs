using SGCM.Applicaction.DTOs.Availability;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces.Availability
{
    public interface IAvailabilityService : IBaseService<
        IAvailabilityService,
        CreateAvailabilityDto,
        UpdateAvailabilityDto, 
        AvailabilityResponse>
    {
        Task<OperationResult<List<AvailabilityResponse>>> GetByDoctorIdAsync(int id);
    }
}
