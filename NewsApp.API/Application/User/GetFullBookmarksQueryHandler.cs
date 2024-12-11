using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Application.Articles;
using NewsApp.API.Application.User.Query;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.User;


internal sealed class GetFullBookmarksQueryHandler : IRequestHandler<GetFullBookmarksQuery, DataCollectionApiResponseDto<ArticleDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFullBookmarksQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
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

    public async Task<DataCollectionApiResponseDto<ArticleDto>> Handle(GetFullBookmarksQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<Data.Entities.User>()
            .GetFirstOrDefaultAsync(e => e.Id == request.UserId);
        
        var articles = await  _unitOfWork.GetRepository<Article>()
            .GetAll(e => user.Saved.Contains(e.Id)).ToListAsync(cancellationToken: cancellationToken);
        
        
        
        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        return new DataCollectionApiResponseDto<ArticleDto>
        {
            Items = articlesDto
        };
        
    }
}