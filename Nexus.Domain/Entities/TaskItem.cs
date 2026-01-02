using Nexus.Domain.Common;

namespace Nexus.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Priority: 0=Low, 1=Medium, 2=High
    public int Priority { get; set; }

    // Status: 0=Todo, 1=In Progress, 2=Done
    public int Status { get; set; }
    
    public DateTime? DueDate { get; set; }

    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}
