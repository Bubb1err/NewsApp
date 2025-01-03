﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models;
using NewsApp.Shared.Models.Base;
using ArticleDto = NewsApp.Shared.Models.Dto.ArticleDto;

namespace NewsApp.API.Application.Articles
{
    public sealed class GetArticlesQuery : IRequest<DataCollectionApiResponseDto<ArticleDto>>
    {
        public ArticleQueryParameters Parameters { get; set; } = new();
    }

    internal sealed class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, DataCollectionApiResponseDto<ArticleDto>>
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const int DEFAULT_PAGE_SIZE = 10;
        private const int MAX_PAGE_SIZE = 50;

        public GetArticlesQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataCollectionApiResponseDto<ArticleDto>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            if (request.Parameters.PageSize <= 0)
            {
                request.Parameters.PageSize = DEFAULT_PAGE_SIZE;
            }
            else if (request.Parameters.PageSize > MAX_PAGE_SIZE)
            {
                request.Parameters.PageSize = MAX_PAGE_SIZE;
            }

            if (request.Parameters.PageNumber <= 0)
            {
                request.Parameters.PageNumber = 1;
            }

            var query = _unitOfWork.GetRepository<Article>().GetAll();

            if (!string.IsNullOrEmpty(request.Parameters.CategoryName))
            {
                query = query.Where(a => a.Category.Name == request.Parameters.CategoryName);
            }

            if (!string.IsNullOrEmpty(request.Parameters.SearchTerm))
            {
                var searchTerm = request.Parameters.SearchTerm.ToLower();
                query = query.Where(a => 
                    a.Title.ToLower().Contains(searchTerm) || 
                    a.Content.ToLower().Contains(searchTerm));
            }

            query = request.Parameters.SortBy?.ToLower() switch
            {
                "title" => request.Parameters.Descending 
                    ? query.OrderByDescending(a => a.Title)
                    : query.OrderBy(a => a.Title),
                "author" => request.Parameters.Descending 
                    ? query.OrderByDescending(a => a.Author)
                    : query.OrderBy(a => a.Author),
                _ => request.Parameters.Descending 
                    ? query.OrderByDescending(a => a.PublishDate)
                    : query.OrderBy(a => a.PublishDate)
            };

            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.Parameters.PageSize);

            if (request.Parameters.PageNumber > totalPages)
            {
                request.Parameters.PageNumber = totalPages;
            }

            var items = await query
                .Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                .Take(request.Parameters.PageSize)
                .Include(a => a.Category)
                .ToListAsync(cancellationToken);

            var articlesDto = _mapper.Map<List<ArticleDto>>(items);

            return new DataCollectionApiResponseDto<ArticleDto>
            {
                Items = articlesDto,
                TotalCount = totalCount,
                PageSize = request.Parameters.PageSize,
                CurrentPage = request.Parameters.PageNumber,
                TotalPages = totalPages
            };
        }
    }
}
