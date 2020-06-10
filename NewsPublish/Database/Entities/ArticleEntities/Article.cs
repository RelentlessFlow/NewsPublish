using System;
using System.Collections.Generic;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.Database.Entities.ArticleEntities
{
    /// <summary>
    /// 文章表
    /// </summary>
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        // 文章封面
        public string CoverPic { get; set; }
        // 文章内容
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        // 文章是否审核通过
        public bool States { get; set; }
        
        // 外键
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }
        
        // Tag标签:多对多
        public List<ArticleTag> ArticleTags { get; set; }
        // 文章报表
        public List<ArticleReviewAudit> Audits { get; set; }

    }
}