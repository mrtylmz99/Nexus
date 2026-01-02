using System.ComponentModel.DataAnnotations;
using Nexus.Domain.Common;
using Nexus.Domain.Enums;
using TaskStatus = Nexus.Domain.Enums.TaskStatus; // Fix ambiguity with System.Threading.Tasks.TaskStatus

namespace Nexus.Domain.Entities;

public class TaskItem : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Enum kullanımı: Veritabanında int olarak tutulur ama kodda okunabilirlik sağlar.
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    
    public DateTime? DueDate { get; set; }

    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}
