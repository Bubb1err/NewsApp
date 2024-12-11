using AutoMapper;
using MediatR;
using NewsApp.API.Application.User.Query;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Application.User;



internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, DataApiResponseDto<UserDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {

        var user = await _unitOfWork.GetRepository<Data.Entities.User>()
            .GetFirstOrDefaultAsync(e => e.Id == request.UserId);
        
        
        var userDto = _mapper.Map<UserDto>(user);

        return new DataApiResponseDto<UserDto>
        {
            Item = userDto,
        };
    }
}