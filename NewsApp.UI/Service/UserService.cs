using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using MudBlazor;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.UI.Service;


public class UserService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly ITokenProvider _tokenProvider;

    
    public UserService(ILocalStorageService localStorage,HttpClient httpClient, ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<Guid> GetUserId()
    {
        return await _localStorage.GetItemAsync<Guid>("userId");
    }

    public async Task<List<Guid>> GetUserLikes()
    {
            var token = await _tokenProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<Guid>>("User/likes");
            return response.Items;
        
        
    }
    
    public async Task<List<Guid>> GetUserBookmarks()
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<Guid>>("User/bookmarks");
        return response.Items;
        
    }
    
    
    
    public async Task<List<ArticleDto>> GetUserFullLikes()
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>("User/likes/full");
        return response.Items;
        
        
    }
    
    public async Task<List<ArticleDto>> GetUserFullBookmarks()
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>("User/bookmarks/full");
        return response.Items ;
        
    }
    
    


    public async Task<bool> UpdateLikes(UpdateDto updateDto)
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PutAsJsonAsync("User/updateLike",updateDto);
        
        return response.IsSuccessStatusCode;

    }
    
    public async Task<bool> UpdateBookmarks(UpdateDto updateDto)
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PutAsJsonAsync("User/updateBookmarks",updateDto);
        
        return response.IsSuccessStatusCode;
        
    }
    
    
    public async Task<UserDto> GetUserInfo()
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataApiResponseDto<UserDto>>("User/info");
        return response.Item;
        
    }


}