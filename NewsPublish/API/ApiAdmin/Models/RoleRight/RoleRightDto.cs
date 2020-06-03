using System;

namespace NewsPublish.API.ApiAdmin.Models.RoleRight
{
    /// <summary>
    /// 查询角色权限时返回的DTO对象
    /// </summary>
    public class RoleRightDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public Guid RightId { get; set; }
        public string RightName { get; set; }
    }
}