using Microsoft.EntityFrameworkCore;
using Nexus.Application.DTOs.Task;
using Nexus.Application.Interfaces;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Persistence;

namespace Nexus.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly NexusDbContext _context;

    public TaskService(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                ProjectId = t.ProjectId,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var t = await _context.TaskItems.FindAsync(id);
        if (t == null) return null;

        return new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            DueDate = t.DueDate,
            ProjectId = t.ProjectId,
            CreatedAt = t.CreatedAt
        };
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto taskDto)
    {
        var task = new TaskItem
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            Priority = taskDto.Priority,
            DueDate = taskDto.DueDate,
            ProjectId = taskDto.ProjectId,
            Status = Domain.Enums.TaskStatus.Todo,
            CreatedAt = DateTime.UtcNow
        };

        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            CreatedAt = task.CreatedAt
        };
    }

    public async Task UpdateTaskStatusAsync(UpdateTaskStatusDto statusDto)
    {
        var task = await _context.TaskItems.FindAsync(statusDto.TaskId);
        if (task != null)
        {
            task.Status = statusDto.Status;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
