namespace Nexus.Application.DTOs.Comment;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty; // For UI convenience
    public DateTime CreatedAt { get; set; }
}
