using MediatR;
using Microsoft.AspNetCore.Identity;

namespace NewsApp.API.Application.User;

public class UpdateUserRoleCommand : IRequest<bool>
{
    public string UserId { get; set; }
    public string NewRole { get; set; }
}

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, bool>
{
    private readonly UserManager<Data.Entities.User> _userManager;
    private readonly ILogger<UpdateUserRoleCommandHandler> _logger;

    public UpdateUserRoleCommandHandler(
        UserManager<Data.Entities.User> userManager,
        ILogger<UpdateUserRoleCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning($"User not found: {request.UserId}");
            return false;
        }

        try
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            var result = await _userManager.AddToRoleAsync(user, request.NewRole);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Role updated for user {user.Email} to {request.NewRole}");
                return true;
            }

            _logger.LogWarning($"Failed to update role for user {user.Email}: {string.Join(", ", result.Errors)}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating role for user {user.Email}");
            throw;
        }
    }
}