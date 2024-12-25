using AutoMapper;
using MediatR;
using NewsApp.API.Application.Articles;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.User;

public class UpdateUserStateCommand : IRequest<DataApiResponseDto<bool>>
{
    public string UserId { get; set; }
    public UserState State { get; set; }
    
}



public class UpdateUserStateCommandHandler : IRequestHandler<UpdateUserStateCommand, DataApiResponseDto<bool>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserStateCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<bool>> Handle(UpdateUserStateCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<Data.Entities.User>().GetFirstOrDefaultAsync(e=>e.Id== request.UserId);

        user.ChangeUserState(request.State);
        
        _unitOfWork.GetRepository<Data.Entities.User>().Update(user);

        Console.WriteLine("User state updated&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
        Console.WriteLine(request.State.ToString());
        Console.WriteLine(user.State.ToString());
        await _unitOfWork.SaveAsync();

        return  new DataApiResponseDto<bool>
        {
            Item = true
        };

    }
}