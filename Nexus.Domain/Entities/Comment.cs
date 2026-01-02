using System.ComponentModel.DataAnnotations;
using Nexus.Domain.Common;

namespace Nexus.Domain.Entities;

public class Comment : BaseEntity
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public int TaskId { get; set; }
    public TaskItem? Task { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
