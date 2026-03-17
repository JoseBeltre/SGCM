using SGCM.Domain.Base;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces;

namespace SGCM.Domain.Services
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly ISystemSettingRepository _settingRepository;

        public SystemSettingService(ISystemSettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<OperationResult<bool>> KeyExistsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return OperationResult<bool>.Failure("Setting key cannot be empty.");

            var result = await _settingRepository.GetByKeyAsync(key);
            if (!result.IsSuccess)
                return OperationResult<bool>.Failure("Couldn't validate setting key.");

            return OperationResult<bool>.Success(
                result.Data != null,
                result.Data != null ? "Setting key exists." : "Setting key not found.");
        }
    }
}