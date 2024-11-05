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
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Controllers;

  [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly AccessControlService _accessControl;

        public CommentController(
            IMediator mediator,
            UserManager<User> userManager,
            AccessControlService accessControl)
        {
            _mediator = mediator;
            _userManager = userManager;
            _accessControl = accessControl;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand createCommentCommand)
        {
            if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Failed to get current user.");
            }

            createCommentCommand.UserId = user.Id; 
            var comment = await _mediator.Send(createCommentCommand);

            return CreatedAtAction(nameof(GetComment), new { id = comment.Item.Content}, comment);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<DataApiResponseDto<CommentDto>>> GetComment(string name)
        {
            var comment = await _mediator.Send(new GetCommentQuery(name));

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(new DataApiResponseDto<CommentDto> { Item = comment.Item });
        }
        
        

      /*  [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentCommand updateCommentCommand)
        {
            if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Failed to get current user.");
            }

            updateCommentCommand.UserId = user.Id; 
            updateCommentCommand.Id = id; 
            await _mediator.Send(updateCommentCommand);

            return NoContent(); 
        }*/

       /* [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Failed to get current user.");
            }

            var deleteCommand = new DeleteCommentCommand(id, user.Id);
            await _mediator.Send(deleteCommand);

            return NoContent(); 
        }*/
        
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
