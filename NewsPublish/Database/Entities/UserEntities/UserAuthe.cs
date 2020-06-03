using System;

namespace NewsPublish.Database.Entities.UserEntities
{
    /// <summary>
    /// 用户授权细腻
    /// </summary>
    public class UserAuthe
    {
        public Guid Id { get; set; }
        // 验证信息
        public string Account { get; set; }
        // 验证方式
        public UserAuthType AuthType { get; set; }
        // 验证密码
        public string Credential { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime ModifyTime { get; set; }
        // 外键
        public User User { get; set; }
        public Guid UserId { get; set; }
    }

}