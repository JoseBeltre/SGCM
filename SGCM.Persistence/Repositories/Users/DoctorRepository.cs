using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Doctor>> AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return OperationResult<Doctor>.Success(doctor);
        }

        public async Task<OperationResult<Doctor?>> GetByIdAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return OperationResult<Doctor?>.Failure("Doctor not found.");
            return OperationResult<Doctor?>.Success(doctor);
        }

        public async Task<OperationResult<List<Doctor>>> GetAllAsync()
        {
            var doctors = await _context.Doctors.ToListAsync();
            return OperationResult<List<Doctor>>.Success(doctors);
        }

        public async Task<OperationResult<Doctor?>> UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
            return OperationResult<Doctor?>.Success(doctor);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return OperationResult.Failure("Doctor not found.");
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            var exists = await _context.Doctors.AnyAsync(x => x.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<Doctor?>> GetByNationalIdAsync(string nationalId)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(x => x.NationalId == nationalId);
            if (doctor == null)
                return OperationResult<Doctor?>.Failure("Doctor not found.");
            return OperationResult<Doctor?>.Success(doctor);
        }

        public async Task<OperationResult<List<Doctor>>> GetActiveAsync()
        {
            var doctors = await _context.Doctors
                .Where(x => x.IsActive)
                .ToListAsync();
            return OperationResult<List<Doctor>>.Success(doctors);
        }

        public async Task<OperationResult<List<Doctor>>> GetDoctorsBySpecialtyIdAsync(int specialtyId)
        {
            var doctors = await _context.Doctors
                .Where(x => x.SpecialtyId == specialtyId && x.IsActive)
                .ToListAsync();
            return OperationResult<List<Doctor>>.Success(doctors);
        }
    }
}