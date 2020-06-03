using AutoMapper;
using NewsPublish.API.ApiSite.Models.Register;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.API.ApiSite.Profiles
{
    /// <summary>
    /// 注册信息DTO的映射配置
    /// AutoMapper配置
    /// </summary>
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterDto, UserAuthe>()
                .ForMember(
                    dest
                        => dest.Credential,
                    opt
                        => opt.MapFrom(src => src.Password));

            CreateMap<RegisterDto, User>();
            
        }
    }
}