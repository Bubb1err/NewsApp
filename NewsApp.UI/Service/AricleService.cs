using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;
using System.Net.Http.Headers;

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
    
    
    public async Task<DataCollectionApiResponseDto<ArticleDto>> GetAllArticlesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>("Articles");
        return response;
        
        
    }
    
    public async Task<DataCollectionApiResponseDto<ArticleDto>> GetPopularArticlesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>("Articles/Popular");
        return response;
        
        
    }
    public async Task<ArticleDto> GetAArticleByIdAsync(Guid articleId)
    {
        var response = await _httpClient.GetFromJsonAsync<DataApiResponseDto<ArticleDto>>($"Articles/{articleId}");
        return response.Item;
        
        
    }

    public async Task<bool> CreateArticleAsync(ArticleDto command)
    {
        try
        {
            var token = await _tokenProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.PostAsJsonAsync("Articles", command);
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