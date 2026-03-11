using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;

namespace SGCM.Persistence.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        public Task<OperationResult<Availability>> AddAsync(Availability entity)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Availability>>> GetActiveByDoctorIdAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Availability>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Availability>>> GetByDoctorIdAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Availability?>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Availability?>> UpdateAsync(Availability entity)
        {
            throw new NotImplementedException();
        }
    }
}
