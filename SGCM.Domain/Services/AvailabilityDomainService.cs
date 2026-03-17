using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    public class AvailabilityDomainService : IAvailabilityDomainService
    {
        private readonly IAvailabilitiesRepository _availabilitiesRepository;
        private readonly IAvailabilityExceptionRepository _availabilitiesExceptionRepository;

        public AvailabilityDomainService(IAvailabilitiesRepository availabilitiesRepository, IAvailabilityExceptionRepository availabilitiesExceptionRepository)
        {
            _availabilitiesRepository = availabilitiesRepository;
            _availabilitiesExceptionRepository = availabilitiesExceptionRepository;
        }

        public async Task<OperationResult<bool>> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime)
        {
            // Verificar si el doctorId es válido
            if (doctorId == 0)
                return OperationResult<bool>.Failure("invalid DoctorId!");
            // Verificar si la fecha de la cita es válida
            if (startTime >= endTime)
                return OperationResult<bool>.Failure("invalid time range!");
            // Obtener las disponibilidades del doctor
            var aResult = await _availabilitiesRepository.GetByDoctorIdAsync(doctorId);
            if (!aResult.IsSuccess)
                return OperationResult<bool>.Failure("Could not retrieve availabilibity");

            var availabilities = aResult.Data ?? new List<Availability>();
            // Obtener el día de la semana de la fecha de la cita
            var dayOfWeek = appointmentDate.DayOfWeek.ToString();
            // Filtrar las disponibilidades del doctor por el día de la semana y si están activas
            var availabilitiesForDays = availabilities.Where(a => a.DayOfWeek.ToString() == dayOfWeek && a.IsActive).ToList();
            if (!availabilitiesForDays.Any()) 
                return OperationResult<bool>.Failure("Error while processing doctor availability");

            // Verificar si alguna de las disponibilidades del doctor coincide con el horario de la cita
            var fitSchedule = availabilitiesForDays.Any(a => a.StartTime <= startTime && a.EndTime >= endTime);
            if (!fitSchedule) 
                return OperationResult<bool>.Failure("Doctor is not available for that date");

            // Obtner las excepciones de disponibilidades del doctor
            var aeResult = await _availabilitiesExceptionRepository.GetByDoctorIdAsync(doctorId);
            if (!aeResult.IsSuccess)
                return OperationResult<bool>.Failure("Could not retrieve doctor exceptions ");
            else if (aeResult.Data == null || !aeResult.Data.Any())
                return OperationResult<bool>.Success(true); // No hay excepciones, doctor disponible

            var availabilitiesExceptions = aeResult.Data;

            // Verificar si alguna de las excepciones de disponibilidades del doctor coincide con el horario de la cita
            var hasException = availabilitiesExceptions.Any(ae => appointmentDate >= ae.StartDate && appointmentDate <= ae.EndDate);
            if (hasException)
                return OperationResult<bool>.Failure("Doctor is not available for that date");

            // Todo nitido, doctor disponible
            return OperationResult<bool>.Success(true);
        }
    }
}
