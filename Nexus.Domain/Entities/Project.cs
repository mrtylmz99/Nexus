using Nexus.Domain.Common;

namespace Nexus.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Status: 0=Active, 1=Completed, 2=Archived
    public int Status { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
