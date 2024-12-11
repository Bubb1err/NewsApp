using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.Articles.Commands;

public record CreateArticleCommand : IRequest<DataApiResponseDto<ArticleDto>>
{ 
    public string Title { get; set; }

    public string Author { get; set; }

    public string Content { get; set; }

} 