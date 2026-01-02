using System.ComponentModel.DataAnnotations;

namespace Nexus.Application.DTOs.Category;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(7)]
    public string ColorCode { get; set; } = "#000000";
}
