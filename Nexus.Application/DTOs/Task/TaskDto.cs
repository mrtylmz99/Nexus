using Nexus.Domain.Enums;
using TaskStatus = Nexus.Domain.Enums.TaskStatus;

namespace Nexus.Application.DTOs.Task;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}
