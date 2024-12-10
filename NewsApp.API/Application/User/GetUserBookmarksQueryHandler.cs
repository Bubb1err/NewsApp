using AutoMapper;
using MediatR;
using NewsApp.API.Application.Articles;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.User;


internal sealed class GetUserBookmarksQueryHandler : IRequestHandler<GetUserBookmarksQuery, DataCollectionApiResponseDto<Guid>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserBookmarksQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

  
    public async Task<DataCollectionApiResponseDto<Guid>> Handle(GetUserBookmarksQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<Data.Entities.User>()
            .GetFirstOrDefaultAsync(e => e.Id == request.UserId);
        
        return new DataCollectionApiResponseDto<Guid>
        {
            Items = user.Saved
        };
    }
}