using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Repository
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<OperationResult<List<Appointment>>> GetByPatientIdAsync(int patientId);
        Task<OperationResult<List<Appointment>>> GetByDoctorIdAsync(int doctorId);
        Task<OperationResult<List<Appointment>>> GetByStatusAsync(AppointmentStatus status);
        Task<OperationResult<List<Appointment>>> GetByDoctorAndDateAsync(int doctorId, DateTime date);
        Task<OperationResult<List<Appointment>>> GetUpcomingConfirmedAsync(int daysAhead);
        Task<OperationResult<bool>> HasConflictAsync(int doctorId, DateTime start, int durationMinutes, int? excludeAppointmentId = null);
    }
}