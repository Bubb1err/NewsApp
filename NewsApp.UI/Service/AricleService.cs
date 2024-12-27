using System.Net;
using NewsApp.Shared.Models.Base;
using System.Net.Http.Headers;
using NewsApp.Shared.Models;
using ArticleDto = NewsApp.Shared.Models.Dto.ArticleDto;
using System.Net.Http.Json;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.UI.Service;

public class AricleService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenProvider;
    private readonly IConfiguration _configuration;

    public AricleService(HttpClient httpClient, ITokenService tokenProvider, IConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _configuration = configuration;
    }

    public async Task<DataCollectionApiResponseDto<ArticleDto>> GetAllArticlesAsync(ArticleQueryParameters parameters)
    {
        try
        {
            var queryString = new List<string>();
            
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
                queryString.Add($"SearchTerm={Uri.EscapeDataString(parameters.SearchTerm)}");
            
            if (!string.IsNullOrEmpty(parameters.CategoryName))
                queryString.Add($"CategoryName={Uri.EscapeDataString(parameters.CategoryName)}");
            
            if (!string.IsNullOrEmpty(parameters.SortBy))
                queryString.Add($"SortBy={parameters.SortBy}");
            
            queryString.Add($"Descending={parameters.Descending}");
            queryString.Add($"PageNumber={parameters.PageNumber}");
            queryString.Add($"PageSize={parameters.PageSize}");

            var url = $"Article?{string.Join("&", queryString)}";
            
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching articles: {response.StatusCode}");
                return new DataCollectionApiResponseDto<ArticleDto>();
            }

            return await response.Content.ReadFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>()
                   ?? new DataCollectionApiResponseDto<ArticleDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching articles: {ex.Message}");
            return new DataCollectionApiResponseDto<ArticleDto>();
        }
    }
    
    public async Task<DataCollectionApiResponseDto<ArticleDto>> GetPopularArticlesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Article/popular");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching popular articles: {response.StatusCode}");
                return new DataCollectionApiResponseDto<ArticleDto>();
            }

            return await response.Content.ReadFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>()
                   ?? new DataCollectionApiResponseDto<ArticleDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching popular articles: {ex.Message}");
            return new DataCollectionApiResponseDto<ArticleDto>();
        }
    }

    public async Task<ArticleDto> GetAArticleByIdAsync(Guid articleId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Article/{articleId}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching article: {response.StatusCode}");
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<DataApiResponseDto<ArticleDto>>();
            return result?.Item;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching article: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateArticleAsync(CreateArticleDto command)
    {
        try
        {
            
            var token = await _tokenProvider.GetTokenAsync();
            Console.WriteLine(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.PostAsJsonAsync("Article", command);
            Console.WriteLine("TEST2");
            Console.WriteLine(response.StatusCode);
            return response.IsSuccessStatusCode;
            
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating article: {ex.Message}");
            return false;
        }
    }

  

    public async Task<bool> UpdateArticleAsync(UpdateArticleDto article)
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync("Article", article);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating article: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteArticleAsync(Guid id)
    {
        Console.WriteLine(id);
        try
        {
            var token = await _tokenProvider.GetTokenAsync();


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteFromJsonAsync<DataApiResponseDto<bool>>($"Article/{id}");
            Console.WriteLine(response.Item);
            return response.Item;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting article: {ex.Message}");
            return false;
        }
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("Article/categories") 
                   ?? new List<CategoryDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting categories: {ex.Message}");
            return new List<CategoryDto>();
        }
    }
}