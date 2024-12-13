using AutoMapper;
using MediatR;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Dto.Coment;

namespace NewsApp.API.Application.Category;

public sealed class CreateCategoryCommand : IRequest<DataApiResponseDto<CategoryDto>>
{
    public string Name { get; set; } = string.Empty;
    
}

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, DataApiResponseDto<CategoryDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {

        var category = new Data.Entities.Category
        {
            Name = request.Name
            
        };
        

            
        await _unitOfWork.GetRepository<Data.Entities.Category>().AddAsync(category);
        await _unitOfWork.SaveAsync();

        var categoryDto = _mapper.Map<CategoryDto>(category);

        return new DataApiResponseDto<CategoryDto>
        {
            Item = categoryDto
        };
    }
}