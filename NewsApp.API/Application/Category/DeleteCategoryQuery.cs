using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.Category;

public class DeleteCategoryQuery(Guid _Id) :IRequest<DataApiResponseDto<bool>>
{
    public Guid Id { get; set; } = _Id;

}

internal sealed class DeleteCategoryQueryHandler : IRequestHandler<DeleteCategoryQuery, DataApiResponseDto<bool>>
{
    private readonly UnitOfWork _unitOfWork;

    private static readonly Guid DefaultCategoryId = new("00000000-0000-0000-0000-000000000000");


    public DeleteCategoryQueryHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    public async Task<DataApiResponseDto<bool>> Handle(DeleteCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.GetRepository<Data.Entities.Category>()
            .GetFirstOrDefaultAsync(c => c.Id == request.Id);
        
        var WorldNewsCategory = await _unitOfWork.GetRepository<Data.Entities.Category>().GetFirstOrDefaultAsync(c => c.Name == "World News");
        
        
        var articles =  _unitOfWork.GetRepository<Article>()
            .GetAll(a => a.CategoryId == category.Id);

        foreach (var article in articles)
        {
            article.CategoryId = WorldNewsCategory.Id;
             _unitOfWork.GetRepository<Article>().Update(article);

        }

        await _unitOfWork.SaveAsync();

        _unitOfWork.GetRepository<Data.Entities.Category>().Delete(category);
        await _unitOfWork.SaveAsync();

        return new DataApiResponseDto<bool>
        {
            Item = true
        };
    }
}