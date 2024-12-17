using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace NewsApp.UI.Service;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
        var claims = jwtToken.Claims.ToList();
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        
        return new AuthenticationState(user);
    }

    public async Task UpdateAuthenticationState(string jwtToken)
    {
        ClaimsPrincipal claimsPrincipal;

        if (string.IsNullOrEmpty(jwtToken))
        {
            await _localStorage.RemoveItemAsync("authToken");
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        }
        else
        {
            await _localStorage.SetItemAsync("authToken", jwtToken);
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken.Claims, "jwt"));
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}