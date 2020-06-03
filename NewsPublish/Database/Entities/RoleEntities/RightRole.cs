using System;
using System.Collections.Generic;

namespace NewsPublish.Database.Entities.RoleEntities
{
    /// <summary>
    /// 权限和角色的多对多映射关系
    /// </summary>
    public class RightRole
    {
        public Guid Id { get; set; }
        public Guid RightId { get; set; }
        public Guid RoleId { get; set; }
        
        public Right Right { get; set; }
        public Role Role { get; set; }
        
        
    }
}