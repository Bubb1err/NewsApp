using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Authentication.UpdateUser;

public sealed class ChangeUserEmailRequestCommand : IRequest<ApiResponseDto>
{
    public required string NewEmail { get; set; }

    public bool SendConfirmationCode { get; set; } = true;

    [JsonIgnore]
    public Data.Entities.User? User { get; set; }
}

public sealed class ChangeUserEmailRequestCommandHandler(UserManager<Data.Entities.User> _userManager) : IRequestHandler<ChangeUserEmailRequestCommand, ApiResponseDto>
{
    public async Task<ApiResponseDto> Handle(ChangeUserEmailRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.User == null)
        {
            throw new ArgumentNullException(nameof(request.User));
        }

        var identityResult = await _userManager.SetEmailAsync(request.User, request.NewEmail);
        if (!identityResult.Succeeded)
        {
            throw new ArgumentException(identityResult.Errors.FirstOrDefault()?.Description);
        }

        identityResult = await _userManager.SetUserNameAsync(request.User, request.NewEmail);
        if (!identityResult.Succeeded)
        {
            throw new ArgumentException(identityResult.Errors.FirstOrDefault()?.Description);
        }

        return new ApiResponseDto();
    }
}
