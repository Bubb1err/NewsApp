using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models;
using NewsApp.Shared.Models.Base;

namespace NewsApp.API.Application.Articles
{
    public sealed class GetPopularArticlesRequest : IRequest<DataCollectionApiResponseDto<NewsApp.Shared.Models.Dto.ArticleDto>>
    {
    }

    internal sealed class GetPopularArticlesRequestHandler : IRequestHandler<GetPopularArticlesRequest, DataCollectionApiResponseDto<NewsApp.Shared.Models.Dto.ArticleDto>>
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPopularArticlesRequestHandler(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<DataCollectionApiResponseDto<NewsApp.Shared.Models.Dto.ArticleDto>> Handle(GetPopularArticlesRequest request, CancellationToken cancellationToken)
        {
            var allUsers = await _unitOfWork.GetRepository<Data.Entities.User>().GetAll().ToListAsync();

            var topArticles = allUsers
                .SelectMany(u => u.Liked)
                .GroupBy(articleId => articleId)
                .Select(g => new
                {
                    ArticleId = g.Key,
                    LikeCount = g.Count()
                })
                .Where(x => x.LikeCount > 0)
                .OrderByDescending(x => x.LikeCount)
                .ToList();

            var topArticleIds = topArticles.Select(x => x.ArticleId).ToList();

            var articles = await _unitOfWork.GetRepository<Article>()
                .GetAll(a => topArticleIds.Contains(a.Id))
                .ToListAsync();

            var articlesDto = _mapper.Map<List<NewsApp.Shared.Models.Dto.ArticleDto>>(articles);

            return new DataCollectionApiResponseDto<NewsApp.Shared.Models.Dto.ArticleDto>
            {
                Items = articlesDto
            };
        }
    }
}
