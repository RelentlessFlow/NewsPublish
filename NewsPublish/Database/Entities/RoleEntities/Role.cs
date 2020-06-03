using System;
using System.Collections.Generic;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.Database.Entities.RoleEntities
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        
        //外键 一对多
        public ICollection<User> Users { get; set; }
        
        // 外键 多对多    功能表
        public ICollection<RightRole> RightRoles { get; set; }
    }
}