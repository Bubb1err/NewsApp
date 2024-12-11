using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Application.User.Query;

public class GetUserQuery :IRequest<DataApiResponseDto<UserDto>>
{
    public string UserId { get; set; }

}