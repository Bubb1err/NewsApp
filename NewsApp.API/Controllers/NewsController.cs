using System.Security.Claims;
using Ardalis.GuardClauses;
using Chatty.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Data.Entities;

namespace NewsApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly UserManager<User> _userManager;

    public NewsController(IMediator mediator, UserManager<User> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
       
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