using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Application.Comments
{
    public sealed class GetCommentQuery(string Id) : IRequest<DataApiResponseDto<CommentDto>>
    {
        public string Id { get; set; } = Id; // The ID of the comment to retrieve

      
    }
}