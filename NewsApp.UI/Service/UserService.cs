using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.UI.Service;


public class UserService
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    
    public UserService(ProtectedSessionStorage sessionStorage,HttpClient httpClient, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    public async Task<Guid> GetUserIdAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<DataApiResponseDto<UserDto>>("User/info");



        return  new Guid( response.Item.Id);
    }

    public async Task<List<Guid>> GetUserLikes()
    {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<Guid>>("User/likes");
            return response.Items;
        
        
    }
    
    public async Task<List<Guid>> GetUserBookmarks()
    {
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<Guid>>("User/bookmarks");
        return response.Items;
        
    }
    
    
    
    public async Task<List<ArticleDto>> GetUserFullLikes()
    {
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>("User/likes/full");
        return response.Items;
        
        
    }
    
    public async Task<List<ArticleDto>> GetUserFullBookmarks()
    {
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<ArticleDto>>("User/bookmarks/full");
        return response.Items ;
        
    }
    
    


    public async Task<bool> UpdateLikes(UpdateDto updateDto)
    {
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PutAsJsonAsync("User/updateLike",updateDto);
        
        return response.IsSuccessStatusCode;

    }
    
    public async Task<bool> UpdateBookmarks(UpdateDto updateDto)
    {
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PutAsJsonAsync("User/updateBookmarks",updateDto);
        
        return response.IsSuccessStatusCode;
        
    }
    
    
    public async Task<UserDto> GetUserInfo()
    {
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try
        {
            var response = await _httpClient.GetFromJsonAsync<DataApiResponseDto<UserDto>>("User/info");
            return response.Item;
        }
        catch (Exception e)
        {
            return new UserDto();
        }
       
        
    }


}