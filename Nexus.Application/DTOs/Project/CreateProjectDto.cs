using System.ComponentModel.DataAnnotations;

namespace Nexus.Application.DTOs.Project;

public class CreateProjectDto
{
    [Required(ErrorMessage = "Proje adı zorunludur.")]
    [MaxLength(100, ErrorMessage = "Proje adı 100 karakterden fazla olamaz.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Açıklama 500 karakterden fazla olamaz.")]
    public string Description { get; set; } = string.Empty;
}
