using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Patient>> AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return OperationResult<Patient>.Success(patient);
        }

        public async Task<OperationResult<Patient?>> GetByIdAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return OperationResult<Patient?>.Failure("Patient not found.");
            return OperationResult<Patient?>.Success(patient);
        }

        public async Task<OperationResult<List<Patient>>> GetAllAsync()
        {
            var patients = await _context.Patients.ToListAsync();
            return OperationResult<List<Patient>>.Success(patients);
        }

        public async Task<OperationResult<Patient?>> UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return OperationResult<Patient?>.Success(patient);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return OperationResult.Failure("Patient not found.");
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult<bool>> ExistsAsync(int id)
        {
            var exists = await _context.Patients.AnyAsync(x => x.Id == id);
            return OperationResult<bool>.Success(exists);
        }

        public async Task<OperationResult<Patient?>> GetByNationalIdAsync(string nationalId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(x => x.NationalId == nationalId);
            if (patient == null)
                return OperationResult<Patient?>.Failure("Patient not found.");
            return OperationResult<Patient?>.Success(patient);
        }

        public async Task<OperationResult<Patient?>> GetByUserIdAsync(int userId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (patient == null)
                return OperationResult<Patient?>.Failure("Patient not found.");
            return OperationResult<Patient?>.Success(patient);
        }
    }
}