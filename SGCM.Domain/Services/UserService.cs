using SGCM.Domain.Base;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public UserService(IUserRepository userRepository, IAppointmentRepository appointmentRepository)
        {
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<OperationResult<bool>> CanBeDeactivatedAsync(int userId)
        {
            var exists = await _userRepository.ExistsAsync(userId);
            if (!exists.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate user.");
            if (!exists.Data)
                return OperationResult<bool>.Failure("User not found.");

            var confirmedAppointments = await _appointmentRepository
                .GetByStatusAsync(AppointmentStatus.Confirmada);
            if (!confirmedAppointments.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate user appointments.");

            var hasActive = confirmedAppointments.Data!
                .Any(a => a.PatientId == userId || a.DoctorId == userId);
            if (hasActive)
                return OperationResult<bool>.Failure(
                    "User cannot be deactivated because they have confirmed appointments.");

            return OperationResult<bool>.Success(true, "User can be safely deactivated.");
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int userId)
        {
            var canDeactivate = await CanBeDeactivatedAsync(userId);
            if (!canDeactivate.IsSuccess || !canDeactivate.Data)
                return OperationResult<bool>.Failure(canDeactivate.Message);

            var result = await _userRepository.GetByIdAsync(userId);
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<bool>.Failure("User not found.");

            result.Data.IsActive = false;
            result.Data.UpdatedAt = DateTime.UtcNow;

            var update = await _userRepository.UpdateAsync(result.Data);
            if (!update.IsSuccess)
                return OperationResult<bool>.Failure(update.Message);

            return OperationResult<bool>.Success(true, "User deactivated successfully.");
        }
    }
}