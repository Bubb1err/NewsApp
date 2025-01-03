﻿using Analytics.Web.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NewsApp.API.Data.Entities;
using NewsApp.API.Data.Repository.Base;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models.Base;
using NewsApp.Shared.Models.Dto.User;

namespace NewsApp.API.Application.Authentication.Register;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, DataApiResponseDto<UserDto>>
{
    private readonly UserManager<Data.Entities.User> _userManager;
    private readonly SignInManager<Data.Entities.User> _signInManager;
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(UserManager<Data.Entities.User> userManager, SignInManager<Data.Entities.User> signInManager, UnitOfWork unitOfWork, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DataApiResponseDto<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new Data.Entities.User { Name = request.Name, Email = request.Email, UserName = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new ArgumentException(result.Errors.FirstOrDefault()?.Description);
        }

        
        await _userManager.UpdateAsync(user);

        var signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            throw new ExecutingException("User sign in failed.", System.Net.HttpStatusCode.BadRequest);
        }

        var userDto = _mapper.Map<UserDto>(user);

        await _userManager.AddToRoleAsync(user, UserRoles.Default);

        return new DataApiResponseDto<UserDto>
        {
            Item = userDto,
        };
    }
}