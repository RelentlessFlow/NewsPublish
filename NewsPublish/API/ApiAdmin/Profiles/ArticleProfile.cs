using AutoMapper;
using NewsPublish.API.ApiAdmin.Models.Article;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.Services.AdminServices.DTO;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// 文章DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleInfoDto>()
                .ForMember(dest => dest.ArticleId,
                    opt => opt.MapFrom(src => src.Id)
                )
                .ForMember(dest => dest.NewsTitle,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content));
            CreateMap<ArticleAddDto, Article>();
            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleUpdateDto, Article>();
        }
    }
}