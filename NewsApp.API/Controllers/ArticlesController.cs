using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Articles;
using NewsApp.API.Application.Articles.Commands;
using NewsApp.API.Data.Entities;
using NewsApp.API.Services;
using NewsApp.API.Constants;
using NewsApp.API.Application.Category;
using NewsApp.Shared.Models;

namespace NewsApp.API.Controllers
{
    
    public class ArticleController(
        IMediator mediator,
        UserManager<User> userManager)
        : NewsController(mediator, userManager)
    {


        [HttpGet]
        public async Task<IActionResult> GetAllArticles([FromQuery] ArticleQueryParameters parameters)
        {
            var getArticlesQuery = new GetArticlesQuery { Parameters = parameters };
            return Ok(await _mediator.Send(getArticlesQuery));
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoryQuery());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllArticleById(Guid id)
        {
            var getArticleByIdQuery = new GetArticleByIdQuery(id);

            return Ok(await _mediator.Send(getArticleByIdQuery));
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetTopNews()
        {
            return Ok(await _mediator.Send(new GetPopularArticlesRequest()));
        }



        
        
        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
            
        }
        
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateArticles([FromBody] UpdateArticleCommand command)
        {
            if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Failed to get current user.");
            }

            if (command.UserId != user.Id && !await userManager.IsInRoleAsync(user, UserRoles.Admin))
            {
                return Unauthorized("Only admins and article authors can update articles.");
            }
            
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:guid}/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticles([FromRoute] Guid id, string userId)
        {
            if (await GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Failed to get current user.");
            }

            if (userId != user.Id && !await userManager.IsInRoleAsync(user, UserRoles.Admin))
            {
                return Unauthorized("Only admins and article authors can delete articles.");
            }
            
            var deleteCategoryQuery = new DeleteArticleQuery(id);
            return Ok(await _mediator.Send(deleteCategoryQuery));
        }
        
        
        
/*
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            // ...
        }

        [Authorize(Roles = $"{UserRoles.Premium},{UserRoles.Admin}")]
        [HttpGet("premium")]
        public async Task<IActionResult> GetPremiumArticles()
        {
            // ...
        }
*/
    }
}
