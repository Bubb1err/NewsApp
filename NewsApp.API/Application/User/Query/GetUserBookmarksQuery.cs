using MediatR;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles;

public class GetUserBookmarksQuery : IRequest<DataCollectionApiResponseDto<Guid>>
{
    public string UserId { get; set; }
}