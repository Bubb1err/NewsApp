using MediatR;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Application.Comments
{
    public sealed class CreateCommentCommand : IRequest<DataApiResponseDto<CommentDto>>
    {
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; 
        public Guid ArticleId { get; set; } 
    }
}