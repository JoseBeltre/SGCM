using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<OperationResult<User?>> GetByEmailAsync(string email);
        Task<OperationResult<bool>> EmailExistsAsync(string email);
        Task<OperationResult<List<User>>> GetByTypeAsync(UserType userType);
    }
}