using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface ISpecialtyRepository : IBaseRepository<Specialty>
    {
        Task<OperationResult<List<Specialty>>> GetActiveAsync(); 
        Task<OperationResult<bool>> DeactivateAsync(int specialtyId);
    }
}
