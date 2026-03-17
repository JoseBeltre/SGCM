using SGCM.Domain.Base;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;
namespace SGCM.Domain.Services
{
    public class SpecialtyDomainService : ISpecialtiyDomainService
    {
        private readonly ISpecialtyRepository _specialtiesRepository;
        private readonly IDoctorRepository _doctorRepository;
        public SpecialtyDomainService(ISpecialtyRepository specialtiesRepository, IDoctorRepository doctorRepository)
        {
            _specialtiesRepository = specialtiesRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult<bool>> CanBeDeactivatedAsync(int specialtyId)
        {
            var exists = await _specialtiesRepository.ExistsAsync(specialtyId);
            if (!exists.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate Specialty");
            else if (!exists.Data)
                return OperationResult<bool>.Failure("Specialty not found");

            var doctorsWithSpecialty = await _doctorRepository.GetDoctorsBySpecialtyIdAsync(specialtyId);
            if (doctorsWithSpecialty.Data.Any())
                return OperationResult<bool>.Failure("Specialty cannot be deactivated because there are doctors associated with it.");

            return OperationResult<bool>.Success(true, "Specialty can be safely deactivated.");
        }
    }
}
