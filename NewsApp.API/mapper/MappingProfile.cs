using AutoMapper;
using NewsApp.API.Data.Entities;
using NewsApp.Shared.Models.Dto;
using NewsApp.Shared.Models.Dto.Coment;
using NewsApp.Shared.Models.Dto.User;

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
        CreateMap<User, UserDto>();
        CreateMap<Category, CategoryDto>();
        
        /*// Обратные маппинги если нужны
        CreateMap<ArticleDto, Article>();
        CreateMap<CommentDto, Comment>();
        CreateMap<UserDto, User>();
        CreateMap<CategoryDto, Category>();*/
    }
}
