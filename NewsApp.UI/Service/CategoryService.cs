using System.Net.Http.Headers;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.UI.Service;

public class CategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    public CategoryService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    public async Task<DataCollectionApiResponseDto<CategoryDto>> GetAllCategories()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<CategoryDto>>("Category");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting categories: {ex.Message}");
            return new DataCollectionApiResponseDto<CategoryDto>();
        }
    }

    public async Task<bool> CreateCategory(CategoryDto category)
    {
        try
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("Category", category);
            Console.WriteLine(response.StatusCode);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating category: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        try
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"Category/{categoryId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting category: {ex.Message}");
            return false;
        }
    }
}