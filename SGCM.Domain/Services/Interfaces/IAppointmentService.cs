using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<OperationResult<bool>> CanBeCancelledAsync(int appointmentId);
        Task<OperationResult<bool>> CanBeConfirmedAsync(int appointmentId);
        Task<OperationResult<bool>> CanBeCompletedAsync(int appointmentId);
        Task<OperationResult<bool>> CanBeRescheduledAsync(int appointmentId, DateTime newDate);
        Task<OperationResult<bool>> HasSchedulingConflictAsync(int doctorId, DateTime date, int durationMinutes, int? excludeAppointmentId = null);
    }
}