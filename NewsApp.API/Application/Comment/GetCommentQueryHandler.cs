using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Application.Comments
{
    internal sealed class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, DataApiResponseDto<CommentDto>>
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCommentQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataApiResponseDto<CommentDto>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.Comment.GetFirstOrDefaultAsync(e => e.User.Name == request.name);

            if (comment == null)
            {
                return new DataApiResponseDto<CommentDto> { Item = null }; // Handle the case where the comment is not found
            }

            // Map the Comment entity to CommentDto
            var commentDto = _mapper.Map<CommentDto>(comment);

            // Return the response with the comment data
            return new DataApiResponseDto<CommentDto>
            {
                Item = commentDto
            };
        }
    }
}