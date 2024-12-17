using System.Security.Claims;
using Ardalis.GuardClauses;
using Chatty.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Comments;
using NewsApp.API.Data.Entities;
using NewsApp.API.Services;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(
    IMediator _mediator,
    AccessControlService accessControlService
) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "Default")]

    public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand createCommentCommand)
    {
        if (await accessControlService.GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
        {
            return BadRequest("Failed to get current user.");
        }

        createCommentCommand.UserId = user.Id;
        var comment = await _mediator.Send(createCommentCommand);

        return CreatedAtAction(nameof(GetComment), new { id = comment.Item.Id }, comment);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]

    public async Task<ActionResult<DataApiResponseDto<CommentDto>>> GetComment(string id)
    {
        var comment = await _mediator.Send(new GetCommentQuery(id));

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(new DataApiResponseDto<CommentDto> { Item = comment.Item });
    }

    [HttpGet("by-article/{articleId}")]
    [AllowAnonymous]

    public async Task<ActionResult<DataApiResponseDto<List<CommentDto>>>> GetCommentsByArticleId(string articleId)
    {
        var comments = await _mediator.Send(new GetCommentsByArticleIdQuery(articleId));

        return Ok(new DataApiResponseDto<List<CommentDto>> { Item = comments });
    }


   
}