using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Application.User;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles;


internal sealed class GetUserLikesQueryHandler : IRequestHandler<GetUserLikesQuery, DataCollectionApiResponseDto<Guid>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserLikesQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataCollectionApiResponseDto<Guid>> Handle(GetUserLikesQuery request, CancellationToken cancellationToken)
    {

        var user = await _unitOfWork.GetRepository<Data.Entities.User>()
            .GetFirstOrDefaultAsync(e => e.Id == request.UserId);
        
        return new DataCollectionApiResponseDto<Guid>
        {
            Items = user.Liked
        };
    }
}