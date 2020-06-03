using AutoMapper;
using NewsPublish.API.ApiAdmin.Models.ArticleTag;
using NewsPublish.Database.Entities.ArticleEntities;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// 文章标签DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class ArticleTagProfile : Profile
    {
        public ArticleTagProfile()
        {
            CreateMap<ArticleTag, ArticleTagDto>();
        }
    }
}