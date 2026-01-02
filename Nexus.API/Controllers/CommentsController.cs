using Microsoft.AspNetCore.Mvc;
using Nexus.Application.DTOs.Comment;
using Nexus.Application.Interfaces;

namespace Nexus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetByTask(int taskId)
    {
        var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentDto commentDto)
    {
        var createdComment = await _commentService.AddCommentAsync(commentDto);
        return Ok(createdComment);
    }
}
