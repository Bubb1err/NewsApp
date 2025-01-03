﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Application.User.Query;

public class GetAllUsersQuery: IRequest<DataCollectionApiResponseDto<UserDto>>
{
    
}


internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, DataCollectionApiResponseDto<UserDto>>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataCollectionApiResponseDto<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {

        var users = await _unitOfWork
            .GetRepository<Data.Entities.User>()
            .GetAll()
            .ToListAsync(cancellationToken: cancellationToken);
        
        var usersDto = _mapper.Map<List<UserDto>>(users);

        return new DataCollectionApiResponseDto<UserDto>
        {
            Items = usersDto
        };
    }
}