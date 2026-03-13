using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly AppDbContext _context;

        public SpecialtyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Specialty>>> GetAllAsync()
        {
            var specialties = await _context.Specialty.ToListAsync();
            return OperationResult<List<Specialty>>.Success(specialties);
        }
        public async Task<OperationResult<Specialty?>> GetByIdAsync(int id)
        {
            var specialty = await _context.Specialty.FindAsync(id);
            if (specialty == null)
                return OperationResult<Specialty?>.Failure("Specialty not found.");

            return OperationResult<Specialty?>.Success(specialty);
        }
        public async Task<OperationResult<List<Specialty>>> GetActiveAsync()
        {
            var specialties = await _context.Specialty.Where(s => s.IsActive).ToListAsync();

            return OperationResult<List<Specialty>>.Success(specialties);
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            bool exists = await _context.Specialty.AnyAsync(s => s.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<Specialty>> AddAsync(Specialty entity)
        {
            await _context.Specialty.AddAsync(entity);
            await _context.SaveChangesAsync();
            return OperationResult<Specialty>.Success(entity);
        }

        public async Task<OperationResult<Specialty?>> UpdateAsync(Specialty entity)
        {
            var existing = await _context.Notifications.FindAsync(entity.Id);
            if (existing == null)
                return OperationResult<Specialty?>.Failure("Notification not found");

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return OperationResult<Specialty?>.Success(entity);
        }

        public async Task<OperationResult<bool>> DeactivateAsync(int specialtyId)
        {
            var entity = await _context.Specialty.FindAsync(specialtyId);
            if (entity == null)
                return OperationResult<bool>.Failure("Specialty not found.");

            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var entity = await _context.Specialty.FindAsync(id);
            if (entity == null)
                return OperationResult.Failure("Specialty not found.");

            if (entity.IsActive)
                return OperationResult.Failure("Cannot delete an active specialty. Please deactivate it first.");

            _context.Specialty.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}