using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Application.Comments;

public class GetCommentsByArticleIdQuery(string articleId) : IRequest<List<CommentDto>>
{
    public string ArticleId { get; set; } = articleId;
}





public class GetCommentsByArticleIdHandler : IRequestHandler<GetCommentsByArticleIdQuery, List<CommentDto>>
{
    
    
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetCommentsByArticleIdHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<List<CommentDto>> Handle(GetCommentsByArticleIdQuery request, CancellationToken cancellationToken)
    {



        var comments = await _unitOfWork.GetRepository<Comment>()
            .GetAll(c => c.ArticleId.ToString() == request.ArticleId)
            .Include(c => c.User)
            
            .ToListAsync(cancellationToken: cancellationToken);
        
        return comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Content = c.Content,
            UserName = c.User.UserName,
            UserId = new Guid(c.UserId),
            
        }).ToList();
    }
}
