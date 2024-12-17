using System.Net.Http.Headers;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.UI.Service;

public class CategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenProvider;

    public CategoryService(HttpClient httpClient, ITokenService tokenProvider)
    {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;
    }

    public async Task<List<CategoryDto>> GetAllCategories()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<CategoryDto>>("Category");
            return response?.Items?.ToList() ?? new List<CategoryDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching categories: {ex.Message}");
            return new List<CategoryDto>();
        }
    }

    public async Task<bool> CreateCategory(string categoryName)
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("Category", new { Name = categoryName });
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating category: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateCategory(Guid categoryId, string newName)
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"Category/{categoryId}", new { Name = newName });
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating category: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"Category/{categoryId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting category: {ex.Message}");
            return false;
        }
    }

    public async Task<CategoryDto?> GetCategoryById(Guid categoryId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<DataApiResponseDto<CategoryDto>>($"Category/{categoryId}");
            return response?.Item;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching category: {ex.Message}");
            return null;
        }
    }
}