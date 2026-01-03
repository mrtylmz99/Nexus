using Nexus.Domain.Entities;
using Nexus.Domain.Enums;
using TaskStatus = Nexus.Domain.Enums.TaskStatus;

namespace Nexus.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(NexusDbContext context)
    {
        context.Database.EnsureCreated();

        // Look for any users. If exist, DB is already seeded.
        // Kullanıcı var mı diye bak. Varsa, veritabanı zaten tohumlanmıştır.
        if (context.Users.Any())
        {
            return;
        }

        // 1. Seed Users (with Hashed Passwords) / Kullanıcıları Ekle (Hash'lenmiş şifrelerle)
        // Note: In a real app, use a service or config for the salt/work factor.
        // Not: Gerçek bir uygulamada salt/work factor için bir servis veya yapılandırma kullanın.
        var users = new User[]
        {
            new User 
            { 
                Username = "jdoe", 
                FullName = "John Doe", 
                Email = "john@nexus.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                CreatedAt = DateTime.UtcNow 
            },
            new User 
            { 
                Username = "asmith", 
                FullName = "Alice Smith", 
                Email = "alice@nexus.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                CreatedAt = DateTime.UtcNow 
            }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // 2. Seed Categories
        var categories = new Category[]
        {
            new Category { Name = "Frontend", ColorCode = "#3B82F6", CreatedAt = DateTime.UtcNow },
            new Category { Name = "Backend", ColorCode = "#10B981", CreatedAt = DateTime.UtcNow },
            new Category { Name = "Bug", ColorCode = "#EF4444", CreatedAt = DateTime.UtcNow }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        // 3. Seed Projects
        var projects = new Project[]
        {
            new Project { Name = "Project Alpha", Description = "The first project", Status = 0, CreatedAt = DateTime.UtcNow },
            new Project { Name = "Project Beta", Description = "Second project", Status = 0, CreatedAt = DateTime.UtcNow }
        };
        context.Projects.AddRange(projects);
        context.SaveChanges();

        // 4. Seed Tasks
        var tasks = new TaskItem[]
        {
            new TaskItem 
            { 
                Title = "Initial Setup", 
                Description = "Setup the project structure", 
                Priority = TaskPriority.High, 
                Status = TaskStatus.Completed, 
                ProjectId = projects[0].Id,
                AssigneeId = users[0].Id,
                CreatedAt = DateTime.UtcNow,
                Categories = new List<Category> { categories[1] } 
            },
            new TaskItem 
            { 
                Title = "Database Design", 
                Description = "Design the schema", 
                Priority = TaskPriority.Medium, 
                Status = TaskStatus.InProgress, 
                ProjectId = projects[0].Id,
                AssigneeId = users[1].Id,
                CreatedAt = DateTime.UtcNow,
                Categories = new List<Category> { categories[1] }
            },
            new TaskItem 
            { 
                Title = "Frontend Init", 
                Description = "Initialize Angular", 
                Priority = TaskPriority.Low, 
                Status = TaskStatus.Todo, 
                ProjectId = projects[1].Id,
                AssigneeId = users[0].Id,
                CreatedAt = DateTime.UtcNow,
                Categories = new List<Category> { categories[0] }
            }
        };

        context.TaskItems.AddRange(tasks);
        context.SaveChanges();
    }
}
