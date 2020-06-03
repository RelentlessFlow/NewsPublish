using AutoMapper;
using NewsPublish.API.ApiAdmin.Models.Role;
using NewsPublish.Database.Entities.RoleEntities;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// 角色DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(
                    dest 
                        => dest.RoleName,
                    opt 
                        => opt.MapFrom(src => src.Name));
            
            CreateMap<RoleAddDto, Role>();
        }
    }
}