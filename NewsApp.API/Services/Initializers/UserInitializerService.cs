using Microsoft.AspNetCore.Identity;
using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Constants;

namespace NewsApp.API.Services.Initializers;

[Service]
public class UserInitializerService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserInitializerService> _logger;

    public UserInitializerService(
        UserManager<User> userManager,
        ILogger<UserInitializerService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Admin user
            await CreateUserIfNotExists(new User
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                Name = "Admin User",
                EmailConfirmed = true
            }, "Admin123!", UserRoles.Admin);

            // Premium user
            await CreateUserIfNotExists(new User
            {
                UserName = "premium@example.com",
                Email = "premium@example.com",
                Name = "Premium User",
                EmailConfirmed = true
            }, "Premium123!", UserRoles.Premium);

            // Default user
            await CreateUserIfNotExists(new User
            {
                UserName = "user@example.com",
                Email = "user@example.com",
                Name = "Default User",
                EmailConfirmed = true
            }, "User123!", UserRoles.Default);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error initializing users: {ex.Message}");
            throw;
        }
    }

    private async Task CreateUserIfNotExists(User user, string password, string role)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        
        if (existingUser != null)
        {
            // Проверяем и исправляем роли существующего пользователя
            var existingRoles = await _userManager.GetRolesAsync(existingUser);
            
            // Удаляем все текущие роли
            if (existingRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(existingUser, existingRoles);
            }
            
            // Добавляем правильную роль
            await _userManager.AddToRoleAsync(existingUser, role);
            _logger.LogInformation($"Updated role for user {user.Email} to {role}");
            return;
        }

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, role);
            _logger.LogInformation($"Created {role} user: {user.Email}");
        }
        else
        {
            _logger.LogError($"Failed to create user {user.Email}: {string.Join(", ", result.Errors)}");
        }
    }
}