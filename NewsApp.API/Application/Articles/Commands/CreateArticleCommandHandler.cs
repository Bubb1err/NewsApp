
using AutoMapper;
using MediatR;
using NewsApp.API.Data;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;

using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.Articles.Commands;

internal sealed class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, DataApiResponseDto<ArticleDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateArticleCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<ArticleDto>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var article = new Article
        {
            Title = request.Title,
            Content = request.Content,
            Author = request.Author,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId,
            PublishDate = DateTime.UtcNow,
            IsPremium = request.IsPremium,
            Id = id,
            SourceUrl = "https://localhost:7220/" + id,
            
        };

            
        await _unitOfWork.GetRepository<Article>().AddAsync(article);
        await _unitOfWork.SaveAsync();

        var articleDto = _mapper.Map<ArticleDto>(article);

        return new DataApiResponseDto<ArticleDto>
        {
            Item = articleDto
        };
        
    }
}