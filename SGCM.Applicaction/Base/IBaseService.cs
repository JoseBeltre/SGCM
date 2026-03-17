using SGCM.Domain.Base;

namespace SGCM.Applicaction.Base
{
    public interface IBaseService<TService, TCreateDto, TUpdateDto, TResponse>
        where TService : class
        where TCreateDto : class
        where TUpdateDto : class
        where TResponse : class
    {
        Task<OperationResult<TResponse>> CreateAsync(TCreateDto createDto);
        Task<OperationResult<TResponse>> GetByIdAsync(int id);
        Task<OperationResult<List<TResponse>>> GetAllAsync();
        Task<OperationResult<TResponse>> UpdateAsync(TUpdateDto updateDto);
        Task<OperationResult> DeleteAsync(int id);
    }
}