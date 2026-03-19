using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class AppointmentHistoryRepository : IAppointmentHistoryRepository
    {
        private readonly AppDbContext _context;

        public AppointmentHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<AppointmentHistory>> AddAsync(AppointmentHistory history)
        {
            await _context.AppointmentHistories.AddAsync(history);
            await _context.SaveChangesAsync();
            return OperationResult<AppointmentHistory>.Success(history);
        }

        public async Task<OperationResult<AppointmentHistory?>> GetByIdAsync(int id)
        {
            var history = await _context.AppointmentHistories.FindAsync(id);
            if (history == null)
                return OperationResult<AppointmentHistory?>.Failure("Appointment history not found.");
            return OperationResult<AppointmentHistory?>.Success(history);
        }

        public async Task<OperationResult<List<AppointmentHistory>>> GetAllAsync()
        {
            var history = await _context.AppointmentHistories.ToListAsync();
            return OperationResult<List<AppointmentHistory>>.Success(history);
        }

        public async Task<OperationResult<AppointmentHistory?>> UpdateAsync(AppointmentHistory history)
        {
            _context.AppointmentHistories.Update(history);
            await _context.SaveChangesAsync();
            return OperationResult<AppointmentHistory?>.Success(history);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var history = await _context.AppointmentHistories.FindAsync(id);
            if (history == null)
                return OperationResult.Failure("Appointment history not found.");
            _context.AppointmentHistories.Remove(history);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            var exists = await _context.AppointmentHistories.AnyAsync(x => x.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<List<AppointmentHistory>>> GetByAppointmentIdAsync(int appointmentId)
        {
            var history = await _context.AppointmentHistories
                .Where(x => x.AppointmentId == appointmentId)
                .OrderBy(x => x.RecordedAt)
                .ToListAsync();
            return OperationResult<List<AppointmentHistory>>.Success(history);
        }
    }
}