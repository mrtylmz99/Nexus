using Nexus.Domain.Common;
using Nexus.Domain.Enums;

namespace Nexus.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public string? ProfilePictureUrl { get; set; }

    public UserStatus Status { get; set; } = UserStatus.Active;

    // Password Reset Properties
    public string? ResetCode { get; set; }
    public DateTime? ResetCodeExpires { get; set; }

    // Navigation property for assigned tasks
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
}
