using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<User>> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return OperationResult<User>.Success(user);
        }

        public async Task<OperationResult<User?>> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return OperationResult<User?>.Failure("User not found.");
            return OperationResult<User?>.Success(user);
        }

        public async Task<OperationResult<List<User>>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return OperationResult<List<User>>.Success(users);
        }

        public async Task<OperationResult<User?>> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return OperationResult<User?>.Success(user);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return OperationResult.Failure("User not found.");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            var exists = await _context.Users.AnyAsync(x => x.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<User?>> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return OperationResult<User?>.Failure("User not found.");
            return OperationResult<User?>.Success(user);
        }

        public async Task<OperationResult<bool>> EmailExistsAsync(string email)
        {
            var exists = await _context.Users.AnyAsync(x => x.Email == email);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<List<User>>> GetByTypeAsync(UserType userType)
        {
            var users = await _context.Users
                .Where(x => x.UserType == userType)
                .ToListAsync();
            return OperationResult<List<User>>.Success(users);
        }
    }
}