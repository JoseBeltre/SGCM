using SGCM.Applicaction.DTOs.User;
using SGCM.Domain.Entities;

namespace SGCM.Applicaction.Mappers
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
                UserType = user.UserType,
                IsActive = user.IsActive,
                LastAccess = user.LastAccess
            };
        }
    }
}