using Nexus.Application.DTOs.Comment;

namespace Nexus.Application.Interfaces;

public interface ICommentService
{
    Task<List<CommentDto>> GetCommentsByTaskIdAsync(int taskId);
    Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto);
}
