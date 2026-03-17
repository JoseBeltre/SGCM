using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Enums;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Appointment>> AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return OperationResult<Appointment>.Success(appointment);
        }

        public async Task<OperationResult<Appointment?>> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return OperationResult<Appointment?>.Failure("Appointment not found.");
            return OperationResult<Appointment?>.Success(appointment);
        }

        public async Task<OperationResult<List<Appointment>>> GetAllAsync()
        {
            var appointments = await _context.Appointments.ToListAsync();
            return OperationResult<List<Appointment>>.Success(appointments);
        }

        public async Task<OperationResult<Appointment?>> UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return OperationResult<Appointment?>.Success(appointment);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return OperationResult.Failure("Appointment not found.");
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            var exists = await _context.Appointments.AnyAsync(x => x.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<List<Appointment>>> GetByPatientIdAsync(int patientId)
        {
            var appointments = await _context.Appointments
                .Where(x => x.PatientId == patientId)
                .ToListAsync();
            return OperationResult<List<Appointment>>.Success(appointments);
        }

        public async Task<OperationResult<List<Appointment>>> GetByDoctorIdAsync(int doctorId)
        {
            var appointments = await _context.Appointments
                .Where(x => x.DoctorId == doctorId)
                .ToListAsync();
            return OperationResult<List<Appointment>>.Success(appointments);
        }

        public async Task<OperationResult<List<Appointment>>> GetByStatusAsync(AppointmentStatus status)
        {
            var appointments = await _context.Appointments
                .Where(x => x.Status == status)
                .ToListAsync();
            return OperationResult<List<Appointment>>.Success(appointments);
        }

        public async Task<OperationResult<List<Appointment>>> GetByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            var appointments = await _context.Appointments
                .Where(x => x.DoctorId == doctorId
                          && x.AppointmentDate.Date == date.Date
                          && x.Status != AppointmentStatus.Cancelada)
                .ToListAsync();
            return OperationResult<List<Appointment>>.Success(appointments);
        }

        public async Task<OperationResult<List<Appointment>>> GetUpcomingConfirmedAsync(int daysAhead)
        {
            var cutoff = DateTime.Now.AddDays(daysAhead);
            var appointments = await _context.Appointments
                .Where(x => x.Status == AppointmentStatus.Confirmada
                          && x.AppointmentDate >= DateTime.Now
                          && x.AppointmentDate <= cutoff)
                .ToListAsync();
            return OperationResult<List<Appointment>>.Success(appointments);
        }

        public async Task<OperationResult<bool>> HasConflictAsync(int doctorId, DateTime start,
            int durationMinutes, int? excludeAppointmentId = null)
        {
            var end = start.AddMinutes(durationMinutes);
            var conflict = await _context.Appointments
                .AnyAsync(x => x.DoctorId == doctorId
                            && (x.Status == AppointmentStatus.Solicitada
                             || x.Status == AppointmentStatus.Confirmada)
                            && (excludeAppointmentId == null || x.Id != excludeAppointmentId)
                            && x.AppointmentDate < end
                            && x.AppointmentDate.AddMinutes(x.DurationMinutes) > start);
            return OperationResult<bool>.Success(conflict);
        }
    }
}