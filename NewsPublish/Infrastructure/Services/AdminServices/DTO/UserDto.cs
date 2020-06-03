using System;

namespace NewsPublish.Infrastructure.Services.AdminServices.DTO
{
    /// <summary>
    /// 从数据库查询用户的详细信息
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        // 昵称
        public string Name { get; set; }
        // 头像URL
        public string Avatar { get; set; }
        public string Introduce { get; set; }
        public string RoleName { get; set; }
        public bool States { get; set; }
    }
}