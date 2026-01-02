using Nexus.Application.DTOs.Project;

namespace Nexus.Application.Interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto projectDto);
    Task DeleteProjectAsync(int id);
}
