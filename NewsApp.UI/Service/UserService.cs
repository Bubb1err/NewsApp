using System.Security.Claims;
using Blazored.LocalStorage;
using MudBlazor;

namespace NewsApp.UI.Service;


public class UserService
{
    private readonly ILocalStorageService _localStorage;

    public UserService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<Guid> GetUserId()
    {
        return await _localStorage.GetItemAsync<Guid>("userId");
    }


}