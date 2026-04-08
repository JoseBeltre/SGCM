using SGCM.Application.Base;
using SGCM.Application.DTOs.Patient;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces
{
    public interface IPatientAppService : IBaseService
        <IPatientAppService,
        AddPatientDto,
        UpdatePatientDto,
        PatientDto>
    {
        Task<OperationResult<PatientDto>> GetByNationalIdAsync(string nationalId);
        Task<OperationResult<PatientDto>> GetByUserIdAsync(int userId);
    }
}