using System;

namespace NewsPublish.Infrastructure.Services.AdminServices.DTO
{
    /// <summary>
    /// 从数据库查询用户账号和角色ID的DTO
    /// </summary>
    public class UserAccountDto
    {
        public string Account { get; set; }
        public string AuthType { get; set; }
        public string Credential { get; set; }
        public Guid RoleId { get; set; }
    }
}