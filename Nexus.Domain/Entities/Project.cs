using System.ComponentModel.DataAnnotations;
using Nexus.Domain.Common;

namespace Nexus.Domain.Entities;

public class Project : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    // Proje durumu için basit bir int kullanıyoruz şimdilik, gerekirse bunu da Enum yapabiliriz.
    // 0=Active, 1=Completed, 2=Archived
    public int Status { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
