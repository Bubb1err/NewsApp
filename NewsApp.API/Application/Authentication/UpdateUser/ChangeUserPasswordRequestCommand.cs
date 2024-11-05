using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Models.Base;

namespace Analytics.API.Application.Authentication.Register;

public sealed class ChangeUserPasswordRequestCommand : IRequest<ApiResponseDto>
{
    public required string NewPassword { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}

public sealed class ChangeUserPasswordRequestCommandHandler(UserManager<User> _userManager) : IRequestHandler<ChangeUserPasswordRequestCommand, ApiResponseDto>
{
    public async Task<ApiResponseDto> Handle(ChangeUserPasswordRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.User == null)
        {
            throw new ArgumentNullException(nameof(request.User));
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(request.User);
        var identityResult = await _userManager.ResetPasswordAsync(request.User, token, request.NewPassword);
        if (!identityResult.Succeeded)
        {
            throw new ArgumentException(identityResult.Errors.FirstOrDefault()?.Description);
        }

        return new ApiResponseDto();
    }
}
