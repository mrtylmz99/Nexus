using Nexus.Application.DTOs.Task;

namespace Nexus.Application.Interfaces;

public interface ITaskService
{
    Task<List<TaskDto>> GetTasksByProjectIdAsync(int projectId);
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto taskDto);
    Task UpdateTaskStatusAsync(UpdateTaskStatusDto statusDto);
}
