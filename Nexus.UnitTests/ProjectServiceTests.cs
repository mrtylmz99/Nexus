using Microsoft.EntityFrameworkCore;
using Nexus.Application.DTOs.Project;
using Nexus.Infrastructure.Persistence;
using Nexus.Infrastructure.Services;
using Nexus.Domain.Entities;

namespace Nexus.UnitTests;

public class ProjectServiceTests
{
    private DbContextOptions<NexusDbContext> CreateNewContextOptions()
    {
        // Use unique database name for each test
        return new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldAddProjectToDatabase()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new NexusDbContext(options);
        var service = new ProjectService(context);

        var dto = new CreateProjectDto
        {
            Name = "Test Project",
            Description = "Test Description"
        };

        // Act
        var result = await service.CreateProjectAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Project", result.Name);
        Assert.NotEqual(0, result.Id);

        // Verify in DB
        using var verifyContext = new NexusDbContext(options);
        var project = await verifyContext.Projects.FirstOrDefaultAsync(p => p.Id == result.Id);
        Assert.NotNull(project);
        Assert.Equal("Test Project", project.Name);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnAllProjects()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using (var context = new NexusDbContext(options))
        {
            context.Projects.Add(new Project { Name = "P1", Status = 0 });
            context.Projects.Add(new Project { Name = "P2", Status = 1 });
            await context.SaveChangesAsync();
        }

        using (var context = new NexusDbContext(options))
        {
            var service = new ProjectService(context);

            // Act
            var results = await service.GetAllProjectsAsync();

            // Assert
            Assert.Equal(2, results.Count);
        }
    }
}
