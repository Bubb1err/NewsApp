using Blazored.LocalStorage;

namespace NewsApp.UI.Service;

public interface ITokenProvider
{
    Task<string> GetTokenAsync();
}

public class TokenProvider : ITokenProvider
{
    private readonly ILocalStorageService _localStorage;

    

    public TokenProvider(ILocalStorageService localStorage)
    {
        
        _localStorage = localStorage;
        
    }

    public async Task<string> GetTokenAsync()
    {
        // Retrieve token from secure storage, cache, or API
        return await _localStorage.GetItemAsync<string>("authToken");
    }
}
