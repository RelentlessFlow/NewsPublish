using AutoMapper;
using NewsPublish.API.ApiAdmin.Models.UserAuthe;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// 用户账号DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class UserAutheProfile : Profile
    {
        public UserAutheProfile()
        {
            CreateMap<UserAuthe, UserAutheDto>();
            CreateMap<UserAutheAddDto, UserAuthe>();
            CreateMap<UserAutheUpdateDto, UserAuthe>();
        }
    }
}