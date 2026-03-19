using SGCM.Domain.Base;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;

        public PatientService(IPatientRepository patientRepository, IUserRepository userRepository)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }

        public async Task<OperationResult<bool>> IsEligibleForAppointmentAsync(int patientId)
        {
            var exists = await _patientRepository.ExistsAsync(patientId);
            if (!exists.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate patient.");
            if (!exists.Data)
                return OperationResult<bool>.Failure("Patient not found.");

            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (!patient.IsSuccess || patient.Data == null)
                return OperationResult<bool>.Failure("Patient not found.");

            var user = await _userRepository.GetByIdAsync(patient.Data.UserId);
            if (!user.IsSuccess || user.Data == null)
                return OperationResult<bool>.Failure("User account not found.");
            if (!user.Data.IsActive)
                return OperationResult<bool>.Failure("Patient account is not active.");

            return OperationResult<bool>.Success(true, "Patient is eligible for an appointment.");
        }
    }
}