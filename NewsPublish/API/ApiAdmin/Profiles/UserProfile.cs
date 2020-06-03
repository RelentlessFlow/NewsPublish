using AutoMapper;
using NewsPublish.API.ApiAdmin.Models.User;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.Services.AdminServices.DTO;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// 用户信息DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                    dest 
                        => dest.Name,
                    opt 
                        => opt.MapFrom(src => src.NickName));

            CreateMap<UserAddDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserUpdateDto>();
        }
    }
}