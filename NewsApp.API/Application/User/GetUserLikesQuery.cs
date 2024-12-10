using MediatR;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.User;

public class GetUserLikesQuery : IRequest<DataCollectionApiResponseDto<Guid>>
{
    public string UserId { get; set; }

}