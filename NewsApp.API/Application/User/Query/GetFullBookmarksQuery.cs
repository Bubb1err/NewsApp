using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.User.Query;

public class GetFullBookmarksQuery : IRequest<DataCollectionApiResponseDto<ArticleDto>>
{
    public string UserId { get; set; }

}