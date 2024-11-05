using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Application.Comments
{
    internal sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, DataApiResponseDto<CommentDto>>
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataApiResponseDto<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                Content = request.Content,
                UserId = request.UserId,
                ArticleId = request.ArticleId,
            };

            
            await _unitOfWork.Comment.AddAsync(comment);
            await _unitOfWork.SaveAsync();

            var commentDto = _mapper.Map<CommentDto>(comment);

            return new DataApiResponseDto<CommentDto>
            {
                Item = commentDto
            };
        }
    }
}