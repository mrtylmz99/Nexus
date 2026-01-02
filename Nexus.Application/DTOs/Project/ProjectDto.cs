using Nexus.Application.DTOs.Task;

namespace Nexus.Application.DTOs.Project;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TaskDto> Tasks { get; set; } = new();
}
