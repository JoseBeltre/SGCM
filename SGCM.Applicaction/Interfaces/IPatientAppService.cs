using SGCM.Applicaction.Base;
using SGCM.Applicaction.DTOs.Patient;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces
{
    public interface IPatientAppService : IBaseService
        <IPatientAppService,
        AddPatientDto,
        UpdatePatientDto,
        PatientDto>
    {
        Task<OperationResult<PatientDto>> GetByNationalIdAsync(string nationalId);
    }
}