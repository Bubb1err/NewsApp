using AutoMapper;
using MediatR;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles;

public class UpdateBookmarksCommandHandler : IRequestHandler<UpdateBookmarksCommand, DataApiResponseDto<bool>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBookmarksCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<bool>> Handle(UpdateBookmarksCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.ToString());
        var user = await _unitOfWork.GetRepository<Data.Entities.User>().GetFirstOrDefaultAsync(e=>e.Id== request.UserId);

        if (request.Value)
        {
            user.AddSaved(request.ArticleId);

        }
        else
        {
            user.RemoveSaved(request.ArticleId);
        }

        Console.WriteLine(user.Saved.ToString());

        _unitOfWork.GetRepository<Data.Entities.User>().Update(user);

        await _unitOfWork.SaveAsync();

        return  new DataApiResponseDto<bool>
        {
            Item = true
        };

    }
}