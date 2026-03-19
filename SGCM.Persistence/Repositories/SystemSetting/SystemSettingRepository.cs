using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly AppDbContext _context;

        public SystemSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<SystemSetting>> AddAsync(SystemSetting setting)
        {
            await _context.SystemSettings.AddAsync(setting);
            await _context.SaveChangesAsync();
            return OperationResult<SystemSetting>.Success(setting);
        }

        public async Task<OperationResult<SystemSetting?>> GetByIdAsync(int id)
        {
            var setting = await _context.SystemSettings.FindAsync(id);
            if (setting == null)
                return OperationResult<SystemSetting?>.Failure("Setting not found.");
            return OperationResult<SystemSetting?>.Success(setting);
        }

        public async Task<OperationResult<List<SystemSetting>>> GetAllAsync()
        {
            var settings = await _context.SystemSettings.ToListAsync();
            return OperationResult<List<SystemSetting>>.Success(settings);
        }

        public async Task<OperationResult<SystemSetting?>> UpdateAsync(SystemSetting setting)
        {
            _context.SystemSettings.Update(setting);
            await _context.SaveChangesAsync();
            return OperationResult<SystemSetting?>.Success(setting);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var setting = await _context.SystemSettings.FindAsync(id);
            if (setting == null)
                return OperationResult.Failure("Setting not found.");
            _context.SystemSettings.Remove(setting);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            var exists = await _context.SystemSettings.AnyAsync(x => x.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<SystemSetting?>> GetByKeyAsync(string key)
        {
            var setting = await _context.SystemSettings
                .FirstOrDefaultAsync(x => x.SettingKey == key);
            if (setting == null)
                return OperationResult<SystemSetting?>.Failure("Setting not found.");
            return OperationResult<SystemSetting?>.Success(setting);
        }
    }
}