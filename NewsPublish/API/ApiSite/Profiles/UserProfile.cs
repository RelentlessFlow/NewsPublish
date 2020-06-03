using AutoMapper;
using NewsPublish.API.ApiSite.Models.User;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.API.ApiSite.Profiles
{
    /// <summary>
    /// 用户DTO的映射配置
    /// AutoMapper配置
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfoDto>();
        }
    }
}