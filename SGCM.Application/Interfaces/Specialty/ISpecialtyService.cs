using SGCM.Application.DTOs.Specialty;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces.Specialty
{
    public interface ISpecialtyService : IBaseService<
        ISpecialtyService,
        CreateSpecialtyDto,
        UpdateSpecialtyDto,
        SpecialtyResponse>
    {
        Task<OperationResult<List<SpecialtyResponse>>> GetActiveAsync();
        Task<OperationResult> DeactivateAsync(int id);
    }
}
