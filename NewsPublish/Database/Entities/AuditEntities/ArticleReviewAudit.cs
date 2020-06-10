using System;
using System.Collections.Generic;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.Database.Entities.AuditEntities
{
    public class ArticleReviewAudit
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 审批之后的备注
        /// </summary>
        public string ReviewRemark { get; set; }
        public DateTime ReviewTime { get; set; }
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
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}