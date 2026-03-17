using SGCM.Domain.Base;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public DoctorService(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<OperationResult<bool>> CanBeDeactivatedAsync(int doctorId)
        {
            var exists = await _doctorRepository.ExistsAsync(doctorId);
            if (!exists.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate doctor.");
            if (!exists.Data)
                return OperationResult<bool>.Failure("Doctor not found.");

            var confirmedAppointments = await _appointmentRepository
                .GetByStatusAsync(AppointmentStatus.Confirmada);
            if (!confirmedAppointments.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate doctor appointments.");

            var hasConfirmed = confirmedAppointments.Data!.Any(a => a.DoctorId == doctorId);
            if (hasConfirmed)
                return OperationResult<bool>.Failure(
                    "Doctor cannot be deactivated because they have confirmed appointments.");

            return OperationResult<bool>.Success(true, "Doctor can be safely deactivated.");
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int doctorId)
        {
            var canDeactivate = await CanBeDeactivatedAsync(doctorId);
            if (!canDeactivate.IsSuccess || !canDeactivate.Data)
                return OperationResult<bool>.Failure(canDeactivate.Message);

            var result = await _doctorRepository.GetByIdAsync(doctorId);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<bool>.Failure("Doctor not found.");

            result.Data.IsActive = false;
            result.Data.UpdatedAt = DateTime.UtcNow;

            var update = await _doctorRepository.UpdateAsync(result.Data);
            if (!update.IsSuccess)
                return OperationResult<bool>.Failure(update.Message);

            return OperationResult<bool>.Success(true, "Doctor deactivated successfully.");
        }
    }
}