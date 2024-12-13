using MediatR;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles;

public class DeleteArticleQuery(Guid id) : IRequest<DataApiResponseDto<bool>>
{
    public Guid Id { get; set; }
    
}

internal sealed class DeleteArticleQueryHandler : IRequestHandler<DeleteArticleQuery, DataApiResponseDto<bool>>
{
    private readonly UnitOfWork _unitOfWork;


    public DeleteArticleQueryHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    public async Task<DataApiResponseDto<bool>> Handle(DeleteArticleQuery request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.GetRepository<Article>()
            .GetFirstOrDefaultAsync(c => c.Id == request.Id);
        _unitOfWork
            .GetRepository<Article>().Delete(article);


        await _unitOfWork.SaveAsync();

        return new DataApiResponseDto<bool>
        {
            Item = true
        };
    }
}