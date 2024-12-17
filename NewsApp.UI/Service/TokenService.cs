using Blazored.LocalStorage;

namespace NewsApp.UI.Service;

public interface ITokenService
{
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string token);
    Task RemoveTokenAsync();


}

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;
    
    public TokenService(ILocalStorageService localStorage)
    {
        
        _localStorage = localStorage;
        
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("authToken");
    }

    public async Task SetTokenAsync(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);
    }

    public async Task RemoveTokenAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
    }
}