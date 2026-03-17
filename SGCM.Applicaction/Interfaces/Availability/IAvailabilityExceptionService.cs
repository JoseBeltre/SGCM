using SGCM.Applicaction.DTOs.Availability;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces.Availability
{
    public interface IAvailabilityExceptionService : IBaseService<
        IAvailabilityExceptionService,
        CreateNotificationDto,
        UpdateNotificationDto,
        NotificationResponse>
    {
        Task<OperationResult<List<NotificationResponse>>> GetByDoctorIdAsync(int id);
    }
}
