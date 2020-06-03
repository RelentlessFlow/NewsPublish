using System;

namespace NewsPublish.API.ApiAdmin.Models.Role
{
    /// <summary>
    /// 查询角色时返回的DTO对象
    /// </summary>
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        
    }
}