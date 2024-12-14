using NewsApp.Shared.Models.Base;
using System.Net.Http.Headers;
using NewsApp.Shared.Models;
using ArticleDto = NewsApp.Shared.Models.Dto.ArticleDto;

namespace NewsApp.UI.Service;

public class AricleService
{
    
    private readonly HttpClient _httpClient;
    private readonly ITokenProvider _tokenProvider;

    
    public AricleService(HttpClient httpClient , ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));

        _httpClient = httpClient;
    }
    
    
    public async Task<DataCollectionApiResponseDto<ArticleDto>> GetAllArticlesAsync(ArticleQueryParameters parameters)
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

        
        return await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>(url) 
               ?? new DataCollectionApiResponseDto<ArticleDto>();
    }
    
    public async Task<DataCollectionApiResponseDto<ArticleDto>> GetPopularArticlesAsync()
    {
        var parameters = new ArticleQueryParameters
        {
            PageSize = 5,
            PageNumber = 1,
            SortBy = "likes",
            Descending = true
        };
        
        var url = $"Article?{string.Join("&", new[] {
            $"PageSize={parameters.PageSize}",
            $"PageNumber={parameters.PageNumber}",
            $"SortBy={parameters.SortBy}",
            $"Descending={parameters.Descending}"
        })}";
        
        return await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>(url) 
               ?? new DataCollectionApiResponseDto<ArticleDto>();
    }
    public async Task<ArticleDto> GetAArticleByIdAsync(Guid articleId)
    {
        var response = await _httpClient.GetFromJsonAsync<DataApiResponseDto<ArticleDto>>($"Article/{articleId}");
        return response.Item;
        
        
    }

    public async Task<bool> CreateArticleAsync(ArticleDto command)
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.PostAsJsonAsync("Article", command);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating article: {ex.Message}");
            return false;
        }
    }
}