using SGCM.Domain.Base;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult<bool>> CanBeCancelledAsync(int appointmentId)
        {
            var result = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (!result.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate appointment.");
            if (result.Data == null)
                return OperationResult<bool>.Failure("Appointment not found.");

            if (result.Data.Status == AppointmentStatus.Completada ||
                result.Data.Status == AppointmentStatus.Cancelada)
                return OperationResult<bool>.Failure(
                    $"Appointment cannot be cancelled because its status is '{result.Data.Status}'.");

            return OperationResult<bool>.Success(true, "Appointment can be cancelled.");
        }

        public async Task<OperationResult<bool>> CanBeConfirmedAsync(int appointmentId)
        {
            var result = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (!result.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate appointment.");
            if (result.Data == null)
                return OperationResult<bool>.Failure("Appointment not found.");

            if (result.Data.Status != AppointmentStatus.Solicitada)
                return OperationResult<bool>.Failure(
                    $"Only requested appointments can be confirmed. Current status: '{result.Data.Status}'.");

            return OperationResult<bool>.Success(true, "Appointment can be confirmed.");
        }

        public async Task<OperationResult<bool>> CanBeCompletedAsync(int appointmentId)
        {
            var result = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (!result.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate appointment.");
            if (result.Data == null)
                return OperationResult<bool>.Failure("Appointment not found.");

            if (result.Data.Status != AppointmentStatus.Confirmada)
                return OperationResult<bool>.Failure(
                    $"Only confirmed appointments can be completed. Current status: '{result.Data.Status}'.");

            return OperationResult<bool>.Success(true, "Appointment can be completed.");
        }

        public async Task<OperationResult<bool>> CanBeRescheduledAsync(int appointmentId, DateTime newDate)
        {
            var result = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (!result.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate appointment.");
            if (result.Data == null)
                return OperationResult<bool>.Failure("Appointment not found.");

            if (result.Data.Status == AppointmentStatus.Completada ||
                result.Data.Status == AppointmentStatus.Cancelada)
                return OperationResult<bool>.Failure(
                    $"Appointment cannot be rescheduled because its status is '{result.Data.Status}'.");

            if (newDate <= DateTime.Now)
                return OperationResult<bool>.Failure("New appointment date must be in the future.");

            var hasConflict = await HasSchedulingConflictAsync(
                result.Data.DoctorId, newDate, result.Data.DurationMinutes, appointmentId);
            if (!hasConflict.IsSuccess)
                return OperationResult<bool>.Failure(hasConflict.Message);
            if (hasConflict.Data)
                return OperationResult<bool>.Failure(
                    "The doctor already has an appointment at the new time.");

            return OperationResult<bool>.Success(true, "Appointment can be rescheduled.");
        }

        public async Task<OperationResult<bool>> HasSchedulingConflictAsync(
            int doctorId, DateTime date, int durationMinutes, int? excludeAppointmentId = null)
        {
            var doctorExists = await _doctorRepository.ExistsAsync(doctorId);
            if (!doctorExists.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate doctor.");
            if (!doctorExists.Data)
                return OperationResult<bool>.Failure("Doctor not found.");

            var conflict = await _appointmentRepository.HasConflictAsync(
                doctorId, date, durationMinutes, excludeAppointmentId);
            if (!conflict.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate scheduling conflict.");

            return OperationResult<bool>.Success(conflict.Data);
        }
    }
}