using Nexus.Domain.Entities;
using Nexus.Domain.Enums;
using TaskStatus = Nexus.Domain.Enums.TaskStatus;

namespace Nexus.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(NexusDbContext context)
    {
        context.Database.EnsureCreated();

        // Look for any projects.
        if (context.Projects.Any())
        {
            return;   // DB has been seeded
        }

        var projects = new Project[]
        {
            new Project
            {
                Name = "Website Redesign",
                Description = "Redesigning the corporate website with new branding.",
                Status = 0, // Active
                CreatedAt = DateTime.UtcNow
            },
            new Project
            {
                Name = "Mobile App Beta",
                Description = "Launch the beta version of the iOS app.",
                Status = 0, // Active
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Projects.AddRange(projects);
        context.SaveChanges();

        var tasks = new TaskItem[]
        {
            new TaskItem
            {
                Title = "Design Mockups",
                Description = "Create Figma mockups for the homepage.",
                Priority = TaskPriority.High,
                Status = TaskStatus.InProgress,
                ProjectId = projects[0].Id,
                CreatedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7)
            },
            new TaskItem
            {
                Title = "Setup DevOps Pipeline",
                Description = "Configure CI/CD actions.",
                Priority = TaskPriority.Medium,
                Status = TaskStatus.Todo,
                ProjectId = projects[1].Id,
                CreatedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3)
            }
        };

        context.TaskItems.AddRange(tasks);
        context.SaveChanges();
    }
}
