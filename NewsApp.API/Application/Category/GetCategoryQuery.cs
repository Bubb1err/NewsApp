using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;

namespace NewsApp.API.Application.Category;

public class GetCategoryQuery : IRequest<DataCollectionApiResponseDto<CategoryDto>>
{
    
}

internal sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, DataCollectionApiResponseDto<CategoryDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataCollectionApiResponseDto<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {

        var categories = await _unitOfWork
            .GetRepository<Data.Entities.Category>()
            .GetAll()
            .ToListAsync(cancellationToken: cancellationToken);
        
        var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

        return new DataCollectionApiResponseDto<CategoryDto>
        {
            Items = categoriesDto
        };
    }
}