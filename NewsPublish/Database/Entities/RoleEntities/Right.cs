using System;
using System.Collections.Generic;

namespace NewsPublish.Database.Entities.RoleEntities
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class Right
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<RightRole> RightRoles { get; set; } 
    }
}