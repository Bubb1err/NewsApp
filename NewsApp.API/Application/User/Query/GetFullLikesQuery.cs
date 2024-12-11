using MediatR;
using NewsApp.Shared.Models;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.User.Query;

public class GetFullLikesQuery : IRequest<DataCollectionApiResponseDto<Shared.Models.Dto.ArticleDto>>
{
    public string UserId { get; set; }

    
}