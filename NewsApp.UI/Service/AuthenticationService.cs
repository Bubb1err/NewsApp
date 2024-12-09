using System.Security.Claims;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.UI.Service;

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

public class CustomAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public CustomAuthenticationService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("Authentication/login", loginDto);

        if (!response.IsSuccessStatusCode) return false;
        
        var result = await response.Content.ReadFromJsonAsync<DataApiResponseDto<AuthenticationResponseDto>>();
        
        await _localStorage.SetItemAsync("authToken", result.Item.JwtToken);
        await _localStorage.SetItemAsync("userId", result.Item.UserId);

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Item.JwtToken);

        return true;

    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("Authentication/register", registerDto);
        return response.IsSuccessStatusCode;
    }


    public async Task<Guid> GetUserId()
    {
        return await _localStorage.GetItemAsync<Guid>("userId");
    }


    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
    
    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (string.IsNullOrEmpty(token))
            return false;

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return false;

        var jwtToken = handler.ReadJwtToken(token);

        if (jwtToken.ValidTo <= DateTime.UtcNow)
            return false;

        return true;
    }
}


