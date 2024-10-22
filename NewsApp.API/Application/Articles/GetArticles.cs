using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.Articles
{
    public sealed class GetArticlesQuery : IRequest<DataCollectionApiResponseDto<ArticleDto>>
    {
    }

    internal sealed class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, DataCollectionApiResponseDto<ArticleDto>>
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetArticlesQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataCollectionApiResponseDto<ArticleDto>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            var articles = await _unitOfWork.Articles.GetAll().ToListAsync();

            var articlesDto = _mapper.Map<List<ArticleDto>>(articles);

            return new DataCollectionApiResponseDto<ArticleDto>
            {
                Items = articlesDto
            };
        }
    }
}
