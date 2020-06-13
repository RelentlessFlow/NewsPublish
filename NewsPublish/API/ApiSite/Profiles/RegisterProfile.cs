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
                        => opt.MapFrom(src => src.Password))
                .ForMember(
                    dest
                        => dest.Account,
                    opt
                        => opt.MapFrom(src => src.Account))
                .ForMember(
                    dest
                        => dest.AuthType,
                    opt
                        => opt.MapFrom(src => src.AuthType));


            CreateMap<RegisterDto, User>()
                .ForMember(
                    dest
                        => dest.NickName,
                    opt
                        => opt.MapFrom(src => src.NickName))
                .ForMember(
                    dest
                        => dest.Introduce,
                    opt
                        => opt.MapFrom(src => src.Introduce));
        }
    }
}