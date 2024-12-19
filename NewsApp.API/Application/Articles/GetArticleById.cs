using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles;


public sealed class GetArticleByIdQuery(Guid id) : IRequest<DataApiResponseDto<ArticleDto>>
{
    public Guid ArticleId { get; set; } = id;
}

internal sealed class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, DataApiResponseDto<ArticleDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetArticleByIdQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<ArticleDto>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork
            .GetRepository<Article>()
            .GetFirstOrDefaultAsync(
                 a => a.Id == request.ArticleId,
                 query => query
                     .Include(a => a.Comments)
                     .Include(a => a.Category)
                   
                
            );


        
        if (article == null)
        {
            return new DataApiResponseDto<ArticleDto>
            {
                Message = "Article not found"
            };
        }

        var articleDto = _mapper.Map<ArticleDto>(article);

        return new DataApiResponseDto<ArticleDto>
        {
            Item = articleDto
        };
    }
}

