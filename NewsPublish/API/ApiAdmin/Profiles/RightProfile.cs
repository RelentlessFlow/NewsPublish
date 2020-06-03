using AutoMapper;
using NewsPublish.API.ApiAdmin.Models.Right;

namespace NewsPublish.API.ApiAdmin.Profiles
{
    /// <summary>
    /// AutoMapper配置文件
    /// 权限DTO映射关系表 左边是需要被转换的对象类型，右边是转换后的对象类型
    /// </summary>
    public class RightProfile : Profile
    {
        public RightProfile()
        {
            CreateMap<Database.Entities.RoleEntities.Right, RightDto>();
        }
    }
}