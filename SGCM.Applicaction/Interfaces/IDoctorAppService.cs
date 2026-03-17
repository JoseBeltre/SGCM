using SGCM.Applicaction.Base;
using SGCM.Applicaction.DTOs.Doctor;
using SGCM.Domain.Base;

namespace SGCM.Applicaction.Interfaces
{
    public interface IDoctorAppService : IBaseService
        <IDoctorAppService,
        AddDoctorDto,
        UpdateDoctorDto,
        DoctorDto>
    {
        Task<OperationResult<List<DoctorDto>>> GetBySpecialtyIdAsync(int specialtyId);
        Task<OperationResult> DeactivateAsync(int id);
    }
}