using System.ComponentModel.DataAnnotations;
using Nexus.Domain.Enums;
using TaskStatus = Nexus.Domain.Enums.TaskStatus;

namespace Nexus.Application.DTOs.Task;

public class CreateTaskDto
{
    [Required(ErrorMessage = "Görev başlığı zorunludur.")]
    [MaxLength(200, ErrorMessage = "Görev başlığı 200 karakterden fazla olamaz.")]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    
    public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = "Proje ID zorunludur.")]
    public int ProjectId { get; set; }
}
