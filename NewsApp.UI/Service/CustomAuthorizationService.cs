using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;
using Microsoft.JSInterop;

namespace NewsApp.UI.Service;


public class CustomAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly ITokenService _tokenService;
    private readonly ILoggingService _logger;

    public CustomAuthenticationService(
        ITokenService tokenService,
        HttpClient httpClient,
        CustomAuthenticationStateProvider authStateProvider,
        ILoggingService logger)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _tokenService = tokenService;
        _logger = logger;
        _logger.LogInfo("CustomAuthenticationService initialized");
    }

    private HttpClient CreateClient()
    {
        _logger.LogDebug("Creating HTTP client");
        return _httpClient;
    }
    
    
    public async Task RefreshTokenAndUpdateState()
    {

        Console.WriteLine("Refreshing token++++++++++++++++++++++++++++++++++++++++");
        try
        {
            var token = await _tokenService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            
            var response = await _httpClient.PostAsync("Authentication/refresh", null);
            Console.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully refreshed");
                var newToken = await response.Content.ReadAsStringAsync();
                Console.WriteLine(newToken);
                await _tokenService.RemoveTokenAsync();
                await  _tokenService.SetTokenAsync(newToken);
                
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                _authStateProvider.UpdateAuthenticationState(newToken);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error refreshing token: {ex.Message}");
        }
    }
    

    public async Task<bool> IsUserAuthenticatedAsync()
    
    {
        _logger.LogDebug("Checking user authentication status");
    
        var token = await GetJwtFromStorage();

        if (!string.IsNullOrEmpty(token))
        {_logger.LogInfo("User authenticated");
            return true;
        }

        _logger.LogInfo("User is not authenticated - no JWT token found");
        return false;

    }

    public async Task<bool> LoginAsync(LoginDto request)
    {
        _logger.LogInfo($"Attempting login for user: {request.Email}");
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("Authentication/login", request);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Login failed for user {request.Email}. Status code: {response.StatusCode}");
            return false;
        }
        
        var result = await response.Content.ReadFromJsonAsync<DataApiResponseDto<AuthenticationResponseDto>>();
        if (result?.Item == null)
        {
            _logger.LogError("Login response was empty or invalid");
            return false;
        }

        await SetJwtToStorage(result.Item.JwtToken);
        
        await _authStateProvider.UpdateAuthenticationState(result.Item.JwtToken);
        
        _logger.LogInfo($"Login successful for user: {request.Email}");
        return true;
    }

    public async Task<bool> RegisterAsync(RegisterDto request)
    {
        _logger.LogInfo($"Attempting registration for user: {request.Email}");
        Console.WriteLine(request.Email);
        Console.WriteLine(request.Password);
        Console.WriteLine(request.ConfirmPassword);
        Console.WriteLine(request.Name);
        var client = CreateClient();
        var response = await _httpClient.PostAsJsonAsync("Authentication/register", request);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Registration failed for user {request.Email}. Status code: {response.StatusCode}");
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<DataApiResponseDto<UserDto>>();
        if (result?.Item == null)
        {
            _logger.LogError("Registration response was empty or invalid");
            return false;
        }

        _logger.LogInfo($"Registration successful for user: {request.Email}. Proceeding with login.");
        return await LoginAsync(new LoginDto { Email = request.Email, Password = request.Password });
    }

    public async Task LogoutAsync()
    {
        _logger.LogInfo("Logging out user");
        await RemoveJwtCookieAsync();
        await _authStateProvider.UpdateAuthenticationState(null);
        _logger.LogInfo("Logout completed successfully");
    }

   

    private async Task SetJwtToStorage(string token)
    {
        _logger.LogDebug("Setting JWT cookie");
        await _tokenService.SetTokenAsync(token);
    }

    public async Task<string?> GetJwtFromStorage()
    {
        _logger.LogDebug("Getting JWT from cookie");
        var token = await _tokenService.GetTokenAsync();
        return token;
    }

    private async Task RemoveJwtCookieAsync()
    {
        _logger.LogDebug("Removing JWT cookie");
        await _tokenService.RemoveTokenAsync();
    }

    

    public async Task<string> AccessInfo()
    {
        var response = await _httpClient.GetAsync("User/access-level");
        
        
        return await response.Content.ReadAsStringAsync();
    }
}