using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using MudBlazor;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;
using ArticleDto = NewsApp.Shared.Models.Dto.ArticleDto;

namespace NewsApp.UI.Service;


public class UserService
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private readonly ITokenService _tokenService;

    
    public UserService(ProtectedSessionStorage sessionStorage,NavigationManager navigationManager ,HttpClient httpClient, ITokenService tokenService,  IJSRuntime jsRuntime)
    {
        _tokenService = tokenService;
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
        _sessionStorage = sessionStorage;
    }

    public async Task<Guid> GetUserIdAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<DataApiResponseDto<UserDto>>("User/info");
        
        return  new Guid( response?.Item.Id ?? throw new InvalidOperationException());
    }

    public async Task<bool> RequestVerification(string userId)
    {
        
        Console.WriteLine("Awaiting verification");
        var token = await _tokenService.GetTokenAsync();
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PutAsJsonAsync("User/verification",userId);
        
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateUserState(UpdateStateDto userState)
    {
        Console.WriteLine("UserState");
        var token = await _tokenService.GetTokenAsync();
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        
        
        var response = await _httpClient.PutAsJsonAsync("User/state",userState  );
        
        return response.IsSuccessStatusCode;
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

    public async Task GetPremium( string url)
    {
        var SubscribrionDto = new SubscriptionRequest();
        
        SubscribrionDto.url = url;
        
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Subscription/create", SubscribrionDto);

            if (response.IsSuccessStatusCode)
            {
                // Десериализуем ответ в объект
                var checkoutResponse = await response.Content.ReadFromJsonAsync<LiqPayResponse>();
                
                if (!string.IsNullOrEmpty(checkoutResponse?.CheckoutUrl))
                {
                    _navigationManager.NavigateTo(checkoutResponse.CheckoutUrl, true);
                }
                else
                {
                    throw new Exception("Checkout URL is empty");
                }
            }
            else
            {
                throw new Exception($"Failed to get checkout URL. Status: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error getting premium subscription: {e.Message}");
            throw;
        }


    }

    public async Task<DataCollectionApiResponseDto<UserDto>> GetAllUsers()
    {
        try
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<UserDto>>("User/all");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting users: {ex.Message}");
            return new DataCollectionApiResponseDto<UserDto>();
        }
    }
    
    public async Task<DataCollectionApiResponseDto<UserDto>> GetAllUsersRequests()
    {
        try
        {
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.GetFromJsonAsync<DataCollectionApiResponseDto<UserDto>>("User/verificationRequests");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting users: {ex.Message}");
            return new DataCollectionApiResponseDto<UserDto>();
        }
    }
    
    

    public async Task<bool> UpdateUserRole(string userId, string role)
    {
        Console.WriteLine("UPDATE USER ROLE");
        var command = new UpdateRoleDto();
        command.UserId = userId;
        command.NewRole = role;
        try
        {
            
            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"User/role", command);
         
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user role: {ex.Message}");
            return false;
        }
    }

}