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

        var users = new User[]
        {
            new User { Username = "jdoe", FullName = "John Doe", Email = "john@nexus.com", CreatedAt = DateTime.UtcNow },
            new User { Username = "asmith", FullName = "Alice Smith", Email = "alice@nexus.com", CreatedAt = DateTime.UtcNow }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        var categories = new Category[]
        {
            new Category { Name = "Frontend", ColorCode = "#3B82F6" },
            new Category { Name = "Backend", ColorCode = "#10B981" },
            new Category { Name = "Bug", ColorCode = "#EF4444" }
        };
        context.Categories.AddRange(categories);
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
                AssigneeId = users[1].Id, // Assign to Alice
                CreatedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7),
                Categories = new List<Category> { categories[0] }
            },
            new TaskItem
            {
                Title = "Setup DevOps Pipeline",
                Description = "Configure CI/CD actions.",
                Priority = TaskPriority.Medium,
                Status = TaskStatus.Todo,
                ProjectId = projects[1].Id,
                AssigneeId = users[0].Id, // Assign to John
                CreatedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3),
                Categories = new List<Category> { categories[1] }
            }
        };

        context.TaskItems.AddRange(tasks);
        context.SaveChanges();
    }
}
