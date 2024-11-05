using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Application.Comments
{
    public sealed class GetCommentQuery(string name) : IRequest<DataApiResponseDto<CommentDto>>
    {
        public string name { get; set; } = name; // The ID of the comment to retrieve

      
    }
}