using AutoMapper;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Dto.Coment;
using NewsApp.Shared.Models.Dto.User;
using Microsoft.AspNetCore.Identity;
using NewsApp.Shared.Constants;

namespace NewsApp.API.mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null ? 
                new CategoryDto 
                { 
                    Id = src.Category.Id, 
                    Name = src.Category.Name 
                } : null));


        CreateMap<Comment, CommentDto>();
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => 
                opt.MapFrom<UserRoleResolver>());
        CreateMap<Category, CategoryDto>();
        
    }
}

public class UserRoleResolver : IValueResolver<User, UserDto, string>
{
    private readonly UserManager<User> _userManager;

    public UserRoleResolver(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
    {
        // Получаем роли пользователя синхронно
        var roles = _userManager.GetRolesAsync(source).Result;
        
        // Возвращаем первую роль или Default если ролей нет
        return roles.FirstOrDefault() ?? UserRoles.Default;
    }
}
