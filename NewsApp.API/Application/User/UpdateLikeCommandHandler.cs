using AutoMapper;
using MediatR;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles;

public class UpdateLikeCommandHandler : IRequestHandler<UpdateLikeCommand, DataApiResponseDto<bool>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLikeCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<bool>> Handle(UpdateLikeCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<Data.Entities.User>().GetFirstOrDefaultAsync(e=>e.Id== request.UserId);
        var article = await _unitOfWork.GetRepository<Article>().GetFirstOrDefaultAsync(e => e.Id == request.ArticleId);

        if (request.Value)
        {
            user.AddLike(request.ArticleId);
            article.AddLike();

        }
        else
        {
            user.RemoveLike(request.ArticleId);
            article.RemoveLike();
        }

        Console.WriteLine(user.Liked.ToString());

        _unitOfWork.GetRepository<Data.Entities.User>().Update(user);
        _unitOfWork.GetRepository<Article>().Update(article);

        await _unitOfWork.SaveAsync();

        return  new DataApiResponseDto<bool>
        {
            Item = true
        };

    }
}