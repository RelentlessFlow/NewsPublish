using System;
using System.Collections.Generic;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Database.Entities.RoleEntities;

namespace NewsPublish.Database.Entities.UserEntities
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        // 昵称
        public string NickName { get; set; }
        // 头像URL
        public string Avatar { get; set; }
        public string Introduce { get; set; }
        public bool States { get; set; }
        
        
        // 外键
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<UserAuthe> UserAuthes { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}