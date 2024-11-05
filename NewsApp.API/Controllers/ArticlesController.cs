using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Articles;
using NewsApp.API.Services;

namespace NewsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllArticles()
        {
            var getArticlesQuery = new GetArticlesQuery();

            return Ok(await _mediator.Send(getArticlesQuery));
        }
    }
}
