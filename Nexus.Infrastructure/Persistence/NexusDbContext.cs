using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;

namespace Nexus.Infrastructure.Persistence;

public class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Project Config
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
    }
}
