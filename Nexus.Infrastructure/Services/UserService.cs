using Microsoft.EntityFrameworkCore;
using Nexus.Application.DTOs.User;
using Nexus.Application.Interfaces;
using Nexus.Infrastructure.Persistence;

namespace Nexus.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly NexusDbContext _context;

    public UserService(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role.ToString(),
                ProfilePictureUrl = u.ProfilePictureUrl,
                Status = u.Status.ToString(),
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new Exception("User not found");

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            ProfilePictureUrl = user.ProfilePictureUrl,
            Status = user.Status.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<bool> ToggleUserStatusAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        // Simple toggle logic: if Active -> Inactive, else -> Active
        if (user.Status == Domain.Enums.UserStatus.Active)
        {
            user.Status = Domain.Enums.UserStatus.Inactive;
        }
        else
        {
            user.Status = Domain.Enums.UserStatus.Active;
        }
        
        await _context.SaveChangesAsync();
        return true;
    }
}
