using MediatR;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles;

public class UpdateLikeCommand : IRequest<DataApiResponseDto<bool>>
{
    
    public string UserId { get; set; }
    public Guid ArticleId { get; set; }
    public bool Value { get; set; }
}