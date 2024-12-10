using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Articles;
using NewsApp.API.Application.User;
using NewsApp.API.Data.Entities;

namespace NewsApp.API.Controllers;


[Route("api/[controller]")]
[Authorize]
public class UserController(IMediator mediator, UserManager<User> userManager) : NewsController(mediator, userManager)
{
    [HttpPut("updateLike")]
    public async Task<IActionResult> UpdateLikeList([FromBody] UpdateLikeCommand updateLikeCommand)
    {
        if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
        {
            return BadRequest("Failed to get current user.");
        }
        updateLikeCommand.UserId = user.Id;
        
        return Ok(await _mediator.Send(updateLikeCommand));
        
    }
    [HttpPut("updateBookmarks")]
    public async Task<IActionResult> UpdateBookmarksList([FromBody] UpdateBookmarksCommand updateBookmarksCommand)
    {
        if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
        {
            return BadRequest("Failed to get current user.");
        }
        updateBookmarksCommand.UserId = user.Id;
        
        return Ok(await _mediator.Send(updateBookmarksCommand));
        
    }
    
    [HttpGet("likes")]
    public async Task<IActionResult> GetAllLikes()
    {
        if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
        {
            return BadRequest("Failed to get current user.");
        }

        
        var getLikesQuery = new GetUserLikesQuery
        {
            UserId = user.Id
        };


        return Ok(await _mediator.Send(getLikesQuery));
    }
    
    
    [HttpGet("bookmarks")]
    public async Task<IActionResult> GetAllBookmarks()
    {
        if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
        {
            return BadRequest("Failed to get current user.");
        }
        
        var getBookmarksQuery = new GetUserBookmarksQuery
        {
            UserId = user.Id
        };


        return Ok(await _mediator.Send(getBookmarksQuery));
    }
    
}