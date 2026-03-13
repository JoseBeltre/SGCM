using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class AvailabilityExceptionRepository : IAvailabilityExceptionRepository
    {
        private readonly AppDbContext _context;

        public AvailabilityExceptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<AvailabilityException>>> GetAllAsync()
        {
            var exceptions = await _context.AvailabilityExceptions.ToListAsync();
            return OperationResult<List<AvailabilityException>>.Success(exceptions);
        }

        public async Task<OperationResult<List<AvailabilityException>>> GetByDoctorIdAsync(int doctorId)
        {
            var exceptions = await _context.AvailabilityExceptions
                .Where(e => e.DoctorId == doctorId)
                .ToListAsync();

            return OperationResult<List<AvailabilityException>>.Success(exceptions);
        }

        public async Task<OperationResult<List<AvailabilityException>>> GetByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate)
        {
            var exceptions = await _context.AvailabilityExceptions
                .Where(e => e.DoctorId == doctorId &&
                            e.StartDate >= startDate &&
                            e.EndDate <= endDate)
                .ToListAsync();

            return OperationResult<List<AvailabilityException>>.Success(exceptions);
        }

        public async Task<OperationResult<AvailabilityException?>> GetByIdAsync(int id)
        {
            var exception = await _context.AvailabilityExceptions.FindAsync(id);
            return OperationResult<AvailabilityException?>.Success(exception);
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            bool exists = await _context.AvailabilityExceptions.AnyAsync(e => e.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<bool>> ExistsInDateAsync(int doctorId, DateTime date)
        {
            bool exists = await _context.AvailabilityExceptions
                .AnyAsync(e =>
                    e.DoctorId == doctorId &&
                    e.StartDate <= date &&
                    e.EndDate >= date);

            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<bool>> ExistsConflictAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            bool exists = await _context.AvailabilityExceptions
                .AnyAsync(e =>
                    e.DoctorId == doctorId &&
                    e.StartDate <= endDate &&
                    e.EndDate >= startDate);

            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<AvailabilityException>> AddAsync(AvailabilityException entity)
        {
            await _context.AvailabilityExceptions.AddAsync(entity);
            await _context.SaveChangesAsync();

            return OperationResult<AvailabilityException>.Success(entity);
        }

        public async Task<OperationResult<AvailabilityException?>> UpdateAsync(AvailabilityException entity)
        {
            var existing = await _context.AvailabilityExceptions.FindAsync(entity.Id);

            if (existing == null)
                return OperationResult<AvailabilityException?>.Failure("AvailabilityException not found");

            _context.Entry(existing).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return OperationResult<AvailabilityException?>.Success(existing);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var entity = await _context.AvailabilityExceptions.FindAsync(id);

            if (entity == null)
                return OperationResult.Failure("AvailabilityException not found");

            _context.AvailabilityExceptions.Remove(entity);
            await _context.SaveChangesAsync();

            return OperationResult.Success();
        }
    }
}