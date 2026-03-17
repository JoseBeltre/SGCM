using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface IPatientRepository : IBaseRepository<Patient>
    {
        Task<OperationResult<Patient?>> GetByNationalIdAsync(string nationalId);
        Task<OperationResult<Patient?>> GetByUserIdAsync(int userId);
    }
}