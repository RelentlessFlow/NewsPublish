using System;
using System.Collections.Generic;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.Database.Entities.ArticleEntities
{
    /// <summary>
    /// 文章评论表
    /// </summary>
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        
        // 外键
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public List<Reply> Replies { get; set; }
    }
}