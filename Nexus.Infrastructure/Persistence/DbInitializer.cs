using Nexus.Domain.Entities;
using Nexus.Domain.Enums;
using TaskStatus = Nexus.Domain.Enums.TaskStatus;

namespace Nexus.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(NexusDbContext context)
    {
        // context.Database.EnsureCreated(); // REMOVED: Using Migrations instead. / Migrations kullanıldığı için kaldırıldı.

        // 1. Users are now seeded via Migrations (NexusDbContext.OnModelCreating)
        // Kullanıcılar artık Migration ile ekleniyor.

        // 2. Seed Categories (if none exist)
        if (!context.Categories.Any())
        {
            var categories = new Category[]
            {
                new Category { Name = "Frontend", ColorCode = "#3B82F6", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Backend", ColorCode = "#10B981", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Bug", ColorCode = "#EF4444", CreatedAt = DateTime.UtcNow }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        // 3. Seed Projects (if none exist)
        if (!context.Projects.Any())
        {
            var projects = new Project[]
            {
                new Project { Name = "Project Alpha", Description = "The first project", Status = 0, CreatedAt = DateTime.UtcNow },
                new Project { Name = "Project Beta", Description = "Second project", Status = 0, CreatedAt = DateTime.UtcNow }
            };
            context.Projects.AddRange(projects);
            context.SaveChanges();
        }

        // 4. Seed Tasks (if none exist AND users/projects exist for linking)
        if (!context.TaskItems.Any() && context.Users.Any() && context.Projects.Any() && context.Categories.Any())
        {
            // Re-fetch required entities to ensure valid IDs
            var adminUser = context.Users.FirstOrDefault(u => u.Email == "admin@nexus.com") ?? context.Users.First();
            var normalUser = context.Users.FirstOrDefault(u => u.Email == "john@nexus.com") ?? context.Users.First();
            var projectAlpha = context.Projects.First(p => p.Name == "Project Alpha");
            var projectBeta = context.Projects.First(p => p.Name == "Project Beta");
            var catFrontend = context.Categories.First(c => c.Name == "Frontend");
            var catBackend = context.Categories.First(c => c.Name == "Backend");

            var tasks = new TaskItem[]
            {
                new TaskItem 
                { 
                    Title = "Initial Setup", 
                    Description = "Setup the project structure", 
                    Priority = TaskPriority.High, 
                    Status = TaskStatus.Completed, 
                    ProjectId = projectAlpha.Id,
                    AssigneeId = adminUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    Categories = new List<Category> { catBackend } 
                },
                new TaskItem 
                { 
                    Title = "Database Design", 
                    Description = "Design the schema", 
                    Priority = TaskPriority.Medium, 
                    Status = TaskStatus.InProgress, 
                    ProjectId = projectAlpha.Id,
                    AssigneeId = normalUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    Categories = new List<Category> { catBackend }
                },
                new TaskItem 
                { 
                    Title = "Frontend Init", 
                    Description = "Initialize Angular", 
                    Priority = TaskPriority.Low, 
                    Status = TaskStatus.Todo, 
                    ProjectId = projectBeta.Id,
                    AssigneeId = adminUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    Categories = new List<Category> { catFrontend }
                }
            };
            context.TaskItems.AddRange(tasks);
            context.SaveChanges();
        }
    }
}
