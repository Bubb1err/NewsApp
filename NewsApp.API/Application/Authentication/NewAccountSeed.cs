using MediatR;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Authentication
{
    public class NewAccountSeedCommand : IRequest<ApiResponseDto>
    {
        public NewAccountSeedCommand(string userId)
        {
            UserId = userId;
        } 

        public string UserId { get; }
    }

    public class NewAccountSeedCommandHandler : IRequestHandler<NewAccountSeedCommand, ApiResponseDto>
    {
        
        public async Task<ApiResponseDto> Handle(NewAccountSeedCommand request, CancellationToken cancellationToken)
        {
         
            return new ApiResponseDto();
        } 
    }
}
