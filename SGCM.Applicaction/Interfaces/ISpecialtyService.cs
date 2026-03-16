using SGCM.Applicaction.DTOs.Specialty;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces
{
    public interface ISpecialtyService
    {
        Task<OperationResult<List<SpecialtyResponse>>> GetAllAsync();
        Task<OperationResult<List<SpecialtyResponse>>> GetActiveAsync();
        Task<OperationResult<SpecialtyResponse>> GetByIdAsync(int id);
        Task<OperationResult<SpecialtyResponse>> CreateAsync(CreateSpecialtyDto request);
        Task<OperationResult<SpecialtyResponse>> UpdateAsync(UpdateSpecialtyDto request);
        Task<OperationResult> DeactivateAsync(int id);
        Task<OperationResult> DeleteAsync(int id);
    }
}
