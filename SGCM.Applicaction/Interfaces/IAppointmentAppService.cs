using SGCM.Applicaction.Base;
using SGCM.Applicaction.DTOs.Appointment;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces
{
    public interface IAppointmentAppService : IBaseService
        <IAppointmentAppService,
        AddAppointmentDto,
        UpdateAppointmentDto,
        AppointmentDto>
    {
        Task<OperationResult<List<AppointmentDto>>> GetByPatientIdAsync(int patientId);
        Task<OperationResult<List<AppointmentDto>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult> ConfirmAsync(int id);
        Task<OperationResult> CancelAsync(int id, string reason);
        Task<OperationResult> CompleteAsync(int id);
    }
}