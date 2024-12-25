using AutoMapper;
using MediatR;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.User;


public class RequestVerificationCommand : IRequest<DataApiResponseDto<bool>>
{
    public string UserId { get; set; }
    
}

public class RequestVerificationCommandHandler : IRequestHandler<RequestVerificationCommand, DataApiResponseDto<bool>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RequestVerificationCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<bool>> Handle(RequestVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<Data.Entities.User>().GetFirstOrDefaultAsync(e=>e.Id== request.UserId);

        user.AwaitingState();
        
        _unitOfWork.GetRepository<Data.Entities.User>().Update(user);

        await _unitOfWork.SaveAsync();

        return  new DataApiResponseDto<bool>
        {
            Item = true
        };

    }

}