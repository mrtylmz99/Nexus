using System.ComponentModel.DataAnnotations;
using Nexus.Domain.Common;

namespace Nexus.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    // Navigation property for assigned tasks
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
}
