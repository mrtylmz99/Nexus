using System.ComponentModel.DataAnnotations;
using Nexus.Domain.Common;

namespace Nexus.Domain.Entities;

public class Category : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(7)] // e.g. #FFFFFF
    public string ColorCode { get; set; } = "#000000";

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
