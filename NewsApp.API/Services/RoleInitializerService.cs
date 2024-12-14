using Microsoft.AspNetCore.Identity;
using NewsApp.API.Constants;
using NewsApp.API.Data.Entities;

namespace NewsApp.API.Services
{
    public class RoleInitializerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleInitializerService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            foreach (var roleName in UserRoles.All)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
} 