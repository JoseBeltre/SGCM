using SGCM.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IAvailabilitiesService
    {
        Task<OperationResult<bool>> IsDoctorAvailableAsync(int doctorId, DateTime appointementDate, TimeSpan startTime, TimeSpan endTime);
    }
}
