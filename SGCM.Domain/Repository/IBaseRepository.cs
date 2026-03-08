using SGCM.Domain.Base;

namespace SGCM.Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity: BaseEntity
    {
        Task<OperationResult<TEntity>> AddAsync(TEntity entity);
        Task<OperationResult<TEntity?>> GetByIdAsync(int id);
        Task<OperationResult<List<TEntity>>> GetAllAsync();
        Task<OperationResult<TEntity?>> UpdateAsync(TEntity entity);
        Task<OperationResult> DeleteAsync(int id);
    }
}
