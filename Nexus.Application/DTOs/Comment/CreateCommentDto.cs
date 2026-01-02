using System.ComponentModel.DataAnnotations;

namespace Nexus.Application.DTOs.Comment;

public class CreateCommentDto
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public int TaskId { get; set; }
    public int UserId { get; set; }
}
