using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles.Commands;

public class UpdateArticleCommand : IRequest<DataApiResponseDto<bool>>
{
    
    public Guid ArticleId { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
}

public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, DataApiResponseDto<bool>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateArticleCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<bool>> Handle(UpdateArticleCommand request,
        CancellationToken cancellationToken)
    {

        var article = await _unitOfWork.GetRepository<Article>().GetFirstOrDefaultAsync(e => e.Id == request.ArticleId);
        
        if (article == null)
        {
            return new DataApiResponseDto<bool>
            {
                Message = "Article not found"
            };
        }
        
        article.Content = request.Content;
        article.Title = request.Title;
            
        _unitOfWork.GetRepository<Article>().Update(article);



        await _unitOfWork.SaveAsync();

        return new DataApiResponseDto<bool>
        {
            Item = true
        };

    }
}
