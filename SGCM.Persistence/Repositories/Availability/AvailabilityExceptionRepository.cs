using SGCM.Domain.Base;
using SGCM.Domain.Entities;
using SGCM.Domain.Repository;

namespace SGCM.Persistence.Repositories
{
    public class AvailabilityExceptionRepository : IAvailabilityExceptionRepository
    {
        public Task<OperationResult<AvailabilityException>> AddAsync(AvailabilityException entity)
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

        public Task<OperationResult<List<AvailabilityException>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<AvailabilityException>>> GetByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<AvailabilityException>>> GetByDoctorIdAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<AvailabilityException?>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<AvailabilityException?>> UpdateAsync(AvailabilityException entity)
        {
            throw new NotImplementedException();
        }
    }
}
