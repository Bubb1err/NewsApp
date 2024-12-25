using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Application.User.Query;


public class GetAwaitingUsersQuery: IRequest<DataCollectionApiResponseDto<UserDto>>
{
    
}


internal sealed class GetAwaitingUsersQueryHandler : IRequestHandler<GetAwaitingUsersQuery, DataCollectionApiResponseDto<UserDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAwaitingUsersQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataCollectionApiResponseDto<UserDto>> Handle(GetAwaitingUsersQuery request, CancellationToken cancellationToken)
    {

        var users = await _unitOfWork
            .GetRepository<Data.Entities.User>()
            .GetAll(e => e.State == UserState.Awaiting)
            .ToListAsync(cancellationToken: cancellationToken);
        
        var usersDto = _mapper.Map<List<UserDto>>(users);

        return new DataCollectionApiResponseDto<UserDto>
        {
            Items = usersDto
        };
    }

}