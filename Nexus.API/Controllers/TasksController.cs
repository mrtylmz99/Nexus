using Microsoft.AspNetCore.Mvc;
using Nexus.Application.DTOs.Task;
using Nexus.Application.Interfaces;

namespace Nexus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProject(int projectId)
    {
        var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto taskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var createdTask = await _taskService.CreateTaskAsync(taskDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(UpdateTaskStatusDto statusDto)
    {
        try
        {
            await _taskService.UpdateTaskStatusAsync(statusDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task status {Id}", statusDto.TaskId);
            return StatusCode(500, "Internal server error");
        }
    }
}
