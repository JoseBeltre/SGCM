using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly AppDbContext _context;

        public AvailabilityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Availability>>> GetAllAsync()
        {
            var availabilities = await _context.Availabilities.ToListAsync();
            return OperationResult<List<Availability>>.Success(availabilities);
        }

        public async Task<OperationResult<List<Availability>>> GetActiveByDoctorIdAsync(int doctorId)
        {
            var availabilities = await _context.Availabilities
                .Where(a => a.DoctorId == doctorId && a.IsActive)
                .ToListAsync();
            return OperationResult<List<Availability>>.Success(availabilities);
        }

        public async Task<OperationResult<List<Availability>>> GetByDoctorIdAsync(int doctorId)
        {
            var availabilities = await _context.Availabilities
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
            return OperationResult<List<Availability>>.Success(availabilities);
        }

        public async Task<OperationResult<Availability?>> GetByIdAsync(int id)
        {
            var availability = await _context.Availabilities.FindAsync(id);
            return OperationResult<Availability?>.Success(availability);
        }
        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            bool exists = await _context.Availabilities.AnyAsync(a => a.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<Availability>> AddAsync(Availability entity)
        {
            await _context.Availabilities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return OperationResult<Availability>.Success(entity);
        }

        public async Task<OperationResult<Availability?>> UpdateAsync(Availability entity)
        {
            var existing = await _context.Availabilities.FindAsync(entity.Id);
            if (existing == null)
                return OperationResult<Availability?>.Failure("Availability not found");

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return OperationResult<Availability?>.Success(existing);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var entity = await _context.Availabilities.FindAsync(id);
            if (entity == null)
                return OperationResult.Failure("Availability not found.");

            _context.Availabilities.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
