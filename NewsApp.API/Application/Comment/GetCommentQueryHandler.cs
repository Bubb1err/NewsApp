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
            var comment = await _unitOfWork.GetRepository<Comment>().GetFirstOrDefaultAsync(e => e.User.Id == request.Id);

            if (comment == null)
            {
                return new DataApiResponseDto<CommentDto> { Item = null };
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            return new DataApiResponseDto<CommentDto>
            {
                Item = commentDto
            };
        }
    }
}