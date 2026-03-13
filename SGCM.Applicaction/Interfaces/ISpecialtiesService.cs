using SGCM.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCM.Domain.Services.Interfaces
{
    public interface ISpecialtiesService
    {
        Task<OperationResult<bool>> CanBeDeactivatedAsync(int specialtyId);
    }
}
