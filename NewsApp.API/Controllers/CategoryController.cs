using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.API.Application.Articles;
using NewsApp.API.Application.Articles.Commands;
using NewsApp.API.Application.Category;

namespace NewsApp.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var getCategoryQuery = new GetCategoryQuery();

        return Ok(await _mediator.Send(getCategoryQuery));
    }
    
    [HttpPost]
    [Authorize] 
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
            
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    {
        var deleteCategoryQuery = new DeleteCategoryQuery(id);

        return Ok(await _mediator.Send(deleteCategoryQuery));

    }


}
