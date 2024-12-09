using System.Security.Claims;

namespace NewsApp.UI.Service;


public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
}