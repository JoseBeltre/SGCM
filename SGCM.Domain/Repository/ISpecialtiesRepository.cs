using SGCM.Domain.Base;
using SGCM.Domain.Entities;

namespace SGCM.Domain.Repository
{
    public interface ISpecialtiesRepository : IBaseRepository<Specialties>
    {
        Task<OperationResult<List<Specialties>>> GetActiveAsync(); 
    }
}
