using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface ISpecialtiyDomainService
    {
        Task<OperationResult<bool>> CanBeDeactivatedAsync(int specialtyId);
    }
}
