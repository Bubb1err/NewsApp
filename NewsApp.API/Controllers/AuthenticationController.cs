﻿using System.Security.Claims;
using Analytics.API.Application.Authentication;
using Analytics.API.Application.Authentication.Register;
using Ardalis.GuardClauses;
using Chatty.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Authentication;
using NewsApp.API.Application.Authentication.Login;
using NewsApp.API.Application.Authentication.Register;
using NewsApp.API.Application.Authentication.UpdateUser;
using NewsApp.API.Data.Entities;
using NewsApp.API.Services;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Controllers;

public class AuthenticationController(
    IMediator mediator,
    UserManager<User> userManager,
    AccessControlService accessControl)
    : NewsController(mediator, userManager)
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
    {
        var user = await _mediator.Send(registerCommand);

        var accountSeed = new NewAccountSeedCommand(user.Item.Id);
        await _mediator.Send(accountSeed);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<DataApiResponseDto<AuthenticationResponseDto>>> Login(
        [FromBody] LoginCommand credential)
    {
        var response = new DataApiResponseDto<AuthenticationResponseDto>();

        if (!ModelState.IsValid)
        {
            return BadRequest(response);
        }

        var userClaims = await accessControl.GetUserClaimsByEmail(credential.Email);
        var passwordHasher = new PasswordHasher<User>();
        var appUser = await _userManager.FindByEmailAsync(credential.Email);
        var result = string.IsNullOrWhiteSpace(appUser?.PasswordHash) is false &&
                     passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, credential.Password) ==
                     PasswordVerificationResult.Success;

        if (!result) return Unauthorized();

        var token = accessControl.GenerateJWTToken(userClaims);

        return Ok(new DataApiResponseDto<AuthenticationResponseDto>
        {
            Item = new AuthenticationResponseDto
            {
                JwtToken = token,
            }
        });
    }
    
    [HttpPost("changeUserPassword")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordRequestCommand changePasswordRequestCommand)
    {
        if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
        {
            return BadRequest("Failed to get current user.");
        }
        changePasswordRequestCommand.User = user;

        await _mediator.Send(changePasswordRequestCommand);

        return Ok(new ApiResponseDto());
    }
   
    
    protected async Task<User?> GetCurrentUser()
    {
        if (User.Identity.IsAuthenticated == false)
        {
            throw new UnauthorizedException("User is not authenticated");
        }

        var id = (User.Identity as ClaimsIdentity).FindFirst(x => x.Type == ClaimTypes.NameIdentifier);

        if (id is null || string.IsNullOrEmpty(id.Value))
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(id!.Value);

        Guard.Against.Null(user, nameof(user));
        return user;
    }
    
}