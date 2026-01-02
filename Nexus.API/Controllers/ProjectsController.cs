using Microsoft.AspNetCore.Mvc;
using Nexus.Application.DTOs.Project;
using Nexus.Application.Interfaces;

namespace Nexus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for creating project");
            return BadRequest(ModelState);
        }

        try
        {
            var createdProject = await _projectService.CreateProjectAsync(projectDto);
            return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
