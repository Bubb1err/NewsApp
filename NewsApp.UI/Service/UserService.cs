using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using MudBlazor;
using NewsApp.Shared.Models.Base;
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
            Console.WriteLine(_httpClient.DefaultRequestHeaders.Authorization.ToString());
            var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<Guid>>("https://localhost:7220/api/User/likes");
            return response.Items;
        
        
    }
    
    public async Task<List<Guid>> GetUserBookmarks()
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<Guid>>("https://localhost:7220/api/User/bookmarks");
        return response.Items;
        
    }


    public async Task<bool> UpdateLikes(UpdateDto updateDto)
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PutAsJsonAsync("https://localhost:7220/api/User/updateLike",updateDto);
        
        return response.IsSuccessStatusCode;

    }
    
    public async Task<bool> UpdateBookmarks(UpdateDto updateDto)
    {
        var token = await _tokenProvider.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7220/api/User/updateBookmarks", updateDto);
        
        return response.IsSuccessStatusCode;
        
    }


}