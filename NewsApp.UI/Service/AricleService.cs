using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.UI.Service;

public class AricleService
{
    
    private readonly HttpClient _httpClient;
    
    public AricleService(HttpClient httpClient)
    {
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

    
}