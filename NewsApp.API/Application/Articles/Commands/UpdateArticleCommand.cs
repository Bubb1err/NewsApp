using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles.Commands;

public class UpdateArticleCommand : IRequest<DataApiResponseDto<ArticleDto>>
{
    public string UserId { get; set; }
    
    public Guid ArticleId { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
}

public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, DataApiResponseDto<ArticleDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateArticleCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<ArticleDto>> Handle(UpdateArticleCommand request,
        CancellationToken cancellationToken)
    {

        var article = await _unitOfWork.GetRepository<Article>().GetFirstOrDefaultAsync(e => e.Id == request.ArticleId);

        if (article.AuthorId == request.UserId)
        {
            article.Update(request.Title, request.Content);

            _unitOfWork.GetRepository<Article>().Update(article);

        }


        await _unitOfWork.SaveAsync();

        var articleDto = _mapper.Map<ArticleDto>(article);

        return new DataApiResponseDto<ArticleDto>
        {
            Item = articleDto
        };

    }
}
