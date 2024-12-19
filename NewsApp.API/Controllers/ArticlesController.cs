using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Articles;
using NewsApp.API.Application.Articles.Commands;
using NewsApp.API.Data.Entities;
using NewsApp.API.Services;
using NewsApp.API.Application.Category;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models;

namespace NewsApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController(
        IMediator mediator,
        UserManager<User> userManager,
        AccessControlService accessControlService)
        : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticles([FromQuery] ArticleQueryParameters parameters)
        {
            var getArticlesQuery = new GetArticlesQuery { Parameters = parameters };
            return Ok(await mediator.Send(getArticlesQuery));
        }

        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await mediator.Send(new GetCategoryQuery());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticleById(Guid id)
        {
            var getArticleByIdQuery = new GetArticleByIdQuery(id);
            return Ok(await mediator.Send(getArticleByIdQuery));
        }

        [HttpGet("popular")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopNews()
        {
            return Ok(await mediator.Send(new GetPopularArticlesRequest()));
        }
        
        [HttpPost]
        [Authorize(Policy = "Premium")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPut]
        [Authorize(Policy = "Premium")]
        public async Task<IActionResult> UpdateArticles([FromBody] UpdateArticleCommand command)
        {
            if (await accessControlService.GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Failed to get current user.");
            }

            
            
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Premium")]
        public async Task<IActionResult> DeleteArticles([FromRoute] Guid id)
        {
            if (await accessControlService.GetCurrentUser() is var user && user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Failed to get current user.");
            }
            
            
            var deleteCategoryQuery = new DeleteArticleQuery(id);
            return Ok(await mediator.Send(deleteCategoryQuery));
        }
    }
}
