using SGCM.Application.Base;
using SGCM.Application.DTOs.Doctor;
using SGCM.Domain.Base;

namespace SGCM.Application.Interfaces
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