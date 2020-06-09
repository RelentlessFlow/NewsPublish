using AutoMapper;
using NewsPublish.API.ApiCommon.Models.Tag;
using NewsPublish.Database.Entities.ArticleEntities;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// 标签DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}