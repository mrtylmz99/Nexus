using Microsoft.EntityFrameworkCore;
using Nexus.Application.DTOs.Category;
using Nexus.Application.Interfaces;
using Nexus.Domain.Entities;
using Nexus.Infrastructure.Persistence;

namespace Nexus.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly NexusDbContext _context;

    public CategoryService(NexusDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        return await _context.Set<Category>()
            .AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ColorCode = c.ColorCode
            })
            .ToListAsync();
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            ColorCode = categoryDto.ColorCode,
            CreatedAt = DateTime.UtcNow
        };

        _context.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ColorCode = category.ColorCode
        };
    }
}
