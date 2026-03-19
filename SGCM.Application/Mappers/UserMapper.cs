using SGCM.Application.DTOs.User;
using SGCM.Domain.Entities;

namespace SGCM.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToResponse(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                UserType = user.UserType.ToString(),
                IsActive = user.IsActive,
                LastAccess = user.LastAccess
            };
        }
    }
}