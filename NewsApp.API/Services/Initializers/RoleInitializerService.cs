using Microsoft.AspNetCore.Identity;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Constants;

namespace NewsApp.API.Services.Initializers
{
    public class RoleInitializerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleInitializerService> _logger;

        public RoleInitializerService(
            RoleManager<IdentityRole> roleManager,
            ILogger<RoleInitializerService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Создаем роли в правильном порядке
                foreach (var roleName in UserRoles.All)
                {
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                        _logger.LogInformation($"Role {roleName} created successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error initializing roles: {ex.Message}");
                throw;
            }
        }
    }
} 