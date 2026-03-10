using SGCM.Domain.Base;
using SGCM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCM.Domain.Services.Interfaces
{
    public interface IAuditLogsService
    {
        Task<OperationResult<bool>> RegisterLogAsync(int userId, EntityType entityType, int entityId, Actions action, string? previousValues, string? newValues, string? ipAddress, string? userAgent);
    }
}
