using SGCM.Applicaction.DTOs.Availability;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces.Availability
{
    public interface IAvailabilityExceptionService : IBaseService<
        IAvailabilityExceptionService,
        CreateAvailabilityExceptionDto,
        UpdateAvailabilityExceptionDto,
        AvailabilityExceptionResponse>
    {
        Task<OperationResult<List<AvailabilityExceptionResponse>>> GetByDoctorIdAsync(int id);
    }
}
