using Microsoft.EntityFrameworkCore;
using Nexus.Infrastructure.Persistence;
using Nexus.Infrastructure.Services;
using Nexus.Domain.Entities;
using Nexus.Domain.Enums;

namespace Nexus.UnitTests;

public class UserServiceTests
{
    private DbContextOptions<NexusDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<NexusDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using (var context = new NexusDbContext(options))
        {
            context.Users.Add(new User { Username = "user1", Email = "u1@test.com", Role = UserRole.User });
            context.Users.Add(new User { Username = "admin", Email = "admin@test.com", Role = UserRole.Admin });
            await context.SaveChangesAsync();
        }

        using (var context = new NexusDbContext(options))
        {
            var service = new UserService(context);

            // Act
            var results = await service.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, results.Count());
            Assert.Contains(results, u => u.Username == "admin");
        }
    }

    [Fact]
    public async Task ToggleUserStatusAsync_ShouldToggleIsActive()
    {
        // Arrange
        var options = CreateNewContextOptions();
        int userId;
        using (var context = new NexusDbContext(options))
        {
            var user = new User { Username = "user1", Email = "u1@test.com", IsActive = true };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            userId = user.Id;
        }

        using (var context = new NexusDbContext(options))
        {
            var service = new UserService(context);

            // Act 1: Deactivate
            var result1 = await service.ToggleUserStatusAsync(userId);
            var userAfter1 = await context.Users.FindAsync(userId);

            // Assert 1
            Assert.True(result1);
            Assert.False(userAfter1!.IsActive);

            // Act 2: Activate
            var result2 = await service.ToggleUserStatusAsync(userId);
            var userAfter2 = await context.Users.FindAsync(userId);

            // Assert 2
            Assert.True(result2);
            Assert.True(userAfter2!.IsActive);
        }
    }
}
