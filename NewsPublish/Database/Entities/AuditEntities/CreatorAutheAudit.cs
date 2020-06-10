using System;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.Database.Entities.AuditEntities
{
    public class CreatorAutheAudit
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 用过提交的申请备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 审批之后的备注
        /// </summary>
        public string ReturnRemark { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否审核通过
        /// </summary>
        public bool IsPass { get; set; } = false;
        /// <summary>
        /// 是否审核完毕
        /// </summary>
        public bool AuditStatus { get; set; } = false;
        // 外键
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}