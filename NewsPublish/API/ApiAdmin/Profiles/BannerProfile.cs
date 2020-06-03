using AutoMapper;
using NewsPublish.API.ApiAdmin.Models.Banner;
using NewsPublish.Database.Entities.WebEntities;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// Banner DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class BannerProfile : Profile
    {
        public BannerProfile()
        {
            CreateMap<BannerAddDto, Banner>();
        }
    }
}