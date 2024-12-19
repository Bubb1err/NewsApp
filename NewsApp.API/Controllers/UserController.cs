using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Articles;
using NewsApp.API.Application.User;
using NewsApp.API.Application.User.Query;
using NewsApp.API.Data.Entities;
using NewsApp.API.Services;
using NewsApp.Shared.Constants;

namespace NewsApp.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AccessControlService _accessControlService;

    public UserController(IMediator mediator, AccessControlService accessControlService)
    {
        _mediator = mediator;
        _accessControlService = accessControlService;
    }

    private async Task<IActionResult> GetCurrentUserOrBadRequest()
    {
        var user = await _accessControlService.GetCurrentUser();
        if (user == null || string.IsNullOrEmpty(user.Id))
        {
            return BadRequest("Failed to get current user.");
        }
        return Ok(user);
    }

    [HttpPut("updateLike")]
    [Authorize(Policy = "Default")]
    public async Task<IActionResult> UpdateLikeList([FromBody] UpdateLikeCommand updateLikeCommand)
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;

        updateLikeCommand.UserId = user.Id;
        return Ok(await _mediator.Send(updateLikeCommand));
    }

    [HttpPut("updateBookmarks")]
    [Authorize(Policy = "Default")]
    public async Task<IActionResult> UpdateBookmarksList([FromBody] UpdateBookmarksCommand updateBookmarksCommand)
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;

        updateBookmarksCommand.UserId = user.Id;
        return Ok(await _mediator.Send(updateBookmarksCommand));
    }

    [HttpGet("likes")]
    [Authorize(Policy = "Default")]
    public async Task<IActionResult> GetAllLikes()
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;

        var getLikesQuery = new GetUserLikesQuery { UserId = user.Id };
        return Ok(await _mediator.Send(getLikesQuery));
    }

    [HttpGet("likes/full")]
    [Authorize(Policy = "Default")]
    public async Task<IActionResult> GetAllLikesFull()
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;

        var getLikesQuery = new GetFullLikesQuery { UserId = user.Id };
        return Ok(await _mediator.Send(getLikesQuery));
    }

    [HttpGet("bookmarks/full")]
    [Authorize(Policy = "Default")]
    public async Task<IActionResult> GetAllBookmarksFull()
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;

        var getBookmarksQuery = new GetFullBookmarksQuery { UserId = user.Id };
        return Ok(await _mediator.Send(getBookmarksQuery));
    }

    [HttpGet("bookmarks")]
    [Authorize(Policy = "Default")]
    public async Task<IActionResult> GetAllBookmarks()
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;

        var getBookmarksQuery = new GetUserBookmarksQuery { UserId = user.Id };
        return Ok(await _mediator.Send(getBookmarksQuery));
    }

    [HttpGet("info")]
    [Authorize(Policy = "Default")]
    public async Task<IActionResult> GetUser()
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;

       

        var getUserQuery = new GetUserQuery { UserId = user.Id };
        return Ok(await _mediator.Send(getUserQuery));
    }

    [HttpGet("access-level")]
    [Authorize(Policy = "Default")]  // Используем политику вместо прямого указания роли
    public async Task<IActionResult> GetUserAccessLevel()
    {
        var userResult = await GetCurrentUserOrBadRequest();
        if (userResult is BadRequestObjectResult) return userResult;
        var user = (userResult as OkObjectResult)?.Value as User;
        
        

        var roles = await _accessControlService.GetUserClaimsByEmail(user.Email);
        
      

        return Ok(roles.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);
    }

    [HttpGet("all")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetAll()
    {

        var getAllQuery = new GetAllUsersQuery();
        return Ok(await _mediator.Send(getAllQuery));
    }

    

    [HttpPut("role")]
    [Authorize(Policy = "Admin")] // Только админы могут менять роли
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleCommand command)
    {
        try 
        {
            // Проверяем существование пользователя
            var user = await _accessControlService.GetUserById(command.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Проверяем валидность роли
            if (!UserRoles.All.Contains(command.NewRole))
            {
                return BadRequest("Invalid role specified");
            }

            return Ok(await _mediator.Send(command));

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}