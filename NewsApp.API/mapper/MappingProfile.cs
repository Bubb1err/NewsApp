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
        CreateMap<Article, ArticleDto>();
        
        CreateMap<User, UserDto>();
        
        CreateMap<Comment, CommentDto>();
        
    }
}
