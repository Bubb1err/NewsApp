using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Application.Articles;
using NewsApp.API.Application.User.Query;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.User;


internal sealed class
    GetFullLikesQueryHandler : IRequestHandler<GetFullLikesQuery, DataCollectionApiResponseDto<ArticleDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFullLikesQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }




    public async Task<DataCollectionApiResponseDto<ArticleDto>> Handle(GetFullLikesQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<Data.Entities.User>()
            .GetFirstOrDefaultAsync(e => e.Id == request.UserId);

        var articles = await _unitOfWork.GetRepository<Article>()
            .GetAll(e => user.Liked.Contains(e.Id)).ToListAsync(cancellationToken: cancellationToken);



        var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

        return new DataCollectionApiResponseDto<ArticleDto>
        {
            Items = articlesDto
        };

    }
}