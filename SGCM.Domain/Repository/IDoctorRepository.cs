using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IDoctorRepository : IBaseRepository<Doctor>
    {
        Task<OperationResult<Doctor?>> GetByNationalIdAsync(string nationalId);
        Task<OperationResult<List<Doctor>>> GetActiveAsync();
        Task<OperationResult<List<Doctor>>> GetDoctorsBySpecialtyIdAsync(int specialtyId);
    }
}