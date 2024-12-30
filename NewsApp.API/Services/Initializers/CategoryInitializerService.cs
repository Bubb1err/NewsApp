using Microsoft.EntityFrameworkCore;
using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

namespace NewsApp.API.Services.Initializers;

[Service]
public class CategoryInitializerService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ILogger<CategoryInitializerService> _logger;
    
    private readonly Dictionary<string, string[]> _defaultCategories = new()
    {
        { "World News", new[] { "BBC", "CNN","cbsnews","yahoo","feedburner","CNBC" } },
        { "Politics", new[] { "FoxNews", "CNN" } },
        { "Technology", new[] { "BBC" } },
        { "Business", new[] { "CNN", "FoxNews" } },
        { "Science", new[] { "BBC" } },
        { "Entertainment", new[] { "FoxNews" } }
    };

    public CategoryInitializerService(UnitOfWork unitOfWork, ILogger<CategoryInitializerService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            foreach (var category in _defaultCategories)
            {
                if (!await _unitOfWork.GetRepository<Category>()
                        .GetAll()
                        .AnyAsync(c => c.Name == category.Key))
                {
                    await _unitOfWork.GetRepository<Category>().AddAsync(new Category
                    {
                        Name = category.Key
                    });
                    _logger.LogInformation($"Created category: {category.Key}");
                }
            }
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing categories");
        }
    }

    public async Task<Category?> GetCategoryForSource(string source)
    {
        var categoryName = _defaultCategories
            .FirstOrDefault(c => c.Value.Contains(source, StringComparer.OrdinalIgnoreCase))
            .Key;

        if (string.IsNullOrEmpty(categoryName))
            return null;

        return await _unitOfWork.GetRepository<Category>()
            .GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == categoryName);
    }
} 