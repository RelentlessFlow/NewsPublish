using System;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.Database.Entities.ArticleEntities
{
    /// <summary>
    /// 评论回复表
    /// </summary>
    public class Reply
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        
        // 外键
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        // 收到回复的人
        public Guid ReceivedId { get; set; }
    }
}