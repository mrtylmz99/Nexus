using Nexus.Domain.Enums;
using TaskStatus = Nexus.Domain.Enums.TaskStatus;

namespace Nexus.Application.DTOs.Task;

public class UpdateTaskStatusDto
{
    public int TaskId { get; set; }
    public TaskStatus Status { get; set; }
}
