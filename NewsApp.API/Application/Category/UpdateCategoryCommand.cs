using AutoMapper;
using MediatR;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.Category;

public class UpdateCategoryCommand : IRequest<DataApiResponseDto<CategoryDto>>
{
    public string Name { get; set; }
    
    public Guid ArticleId { get; set; }
    
    public bool Value { get; set; }
    
    
}


public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, DataApiResponseDto<CategoryDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.GetRepository<Article>().GetFirstOrDefaultAsync(e => e.Id == request.ArticleId);
        var category = await _unitOfWork.GetRepository<Data.Entities.Category>().GetFirstOrDefaultAsync(e => e.Name == request.Name);
        
        
        if (request.Value)
        {
            category.AddArticle(article);
            article.Category = category;
        }
        else
        {
            category.RemoveArticle(article);
            article.Category = null;
        }

        _unitOfWork.GetRepository<Data.Entities.Category>().Update(category);
        _unitOfWork.GetRepository<Article>().Update(article);

        await _unitOfWork.SaveAsync();
        
        var categoryDto = _mapper.Map<CategoryDto>(category);

        
        return  new DataApiResponseDto<CategoryDto>
        {
            Item = categoryDto
        };

    }
}