using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using BCrypt.Net;

namespace Nexus.Infrastructure.Persistence;

public class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly (Infrastructure)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);

        // Project Config (Keep existing if not moved to separate config, or better yet, move them later)
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Project>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // TaskItem Config
        modelBuilder.Entity<TaskItem>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        // Task - Category Many-to-Many
        modelBuilder.Entity<TaskItem>()
            .HasMany(t => t.Categories)
            .WithMany(c => c.Tasks)
            .UsingEntity(j => j.ToTable("TaskCategories"));
            
        // Comment Config
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction); // Avoid cycles or multiple cascade paths

        // SEED DATA
        
        // 1. Users
        var adminId = 1;
        var userId = 2;
        var managerId = 3;

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = adminId,
                Username = "admin",
                FullName = "System Administrator",
                Email = "admin@nexus.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = Domain.Enums.UserRole.Admin,
                Status = Domain.Enums.UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = userId,
                Username = "jdoe",
                FullName = "John Doe",
                Email = "john@nexus.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = Domain.Enums.UserRole.User,
                Status = Domain.Enums.UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = managerId,
                Username = "asmith",
                FullName = "Alice Smith",
                Email = "alice@nexus.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = Domain.Enums.UserRole.Manager,
                Status = Domain.Enums.UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
}
}
