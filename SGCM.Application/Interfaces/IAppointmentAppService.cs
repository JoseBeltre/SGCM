using SGCM.Application.Base;
using SGCM.Application.DTOs.Appointment;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces
{
    public interface IAppointmentAppService : IBaseService
        <IAppointmentAppService,
        AddAppointmentDto,
        UpdateAppointmentDto,
        AppointmentDto>
    {
        Task<OperationResult<List<AppointmentDto>>> GetByPatientIdAsync(int patientId);
        Task<OperationResult<List<AppointmentDto>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult<List<AppointmentDto>>> GetByDoctorAndDateAsync(int doctorId, DateTime date);
        Task<OperationResult> RescheduleAsync(int id, DateTime newDate);
        Task<OperationResult> ConfirmAsync(int id);
        Task<OperationResult> CancelAsync(int id, string reason);
        Task<OperationResult> CompleteAsync(int id);
    }
}