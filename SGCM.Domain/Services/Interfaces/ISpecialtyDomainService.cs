using SGCM.Domain.Base;

namespace SGCM.Domain.Services.Interfaces
{
    public interface ISpecialtyDomainService
    {
        Task<OperationResult<bool>> CanBeDeactivatedAsync(int specialtyId);
    }
}
