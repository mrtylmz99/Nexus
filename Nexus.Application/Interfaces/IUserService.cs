using Nexus.Application.DTOs.User;

namespace Nexus.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task<bool> ToggleUserStatusAsync(int id); // Ban/Activate
}
