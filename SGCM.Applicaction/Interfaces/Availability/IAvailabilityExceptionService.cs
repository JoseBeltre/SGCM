using SGCM.Application.DTOs.Availability;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces.Availability
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
