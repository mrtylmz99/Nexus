using Microsoft.EntityFrameworkCore;
using Nexus.Application.DTOs.Comment;
using Nexus.Application.Interfaces;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Persistence;

namespace Nexus.Infrastructure.Services;

public class CommentService : ICommentService
{
    private readonly NexusDbContext _context;

    public CommentService(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<List<CommentDto>> GetCommentsByTaskIdAsync(int taskId)
    {
        return await _context.Set<Comment>()
            .AsNoTracking()
            .Where(c => c.TaskId == taskId)
            .Include(c => c.User)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                TaskId = c.TaskId,
                UserId = c.UserId,
                Username = c.User != null ? c.User.Username : "Unknown",
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto)
    {
        var comment = new Comment
        {
            Content = commentDto.Content,
            TaskId = commentDto.TaskId,
            UserId = commentDto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Add(comment);
        await _context.SaveChangesAsync();

        // Load User to return properly
        await _context.Entry(comment).Reference(c => c.User).LoadAsync();

        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            TaskId = comment.TaskId,
            UserId = comment.UserId,
            Username = comment.User?.Username ?? "Unknown",
            CreatedAt = comment.CreatedAt
        };
    }
}
