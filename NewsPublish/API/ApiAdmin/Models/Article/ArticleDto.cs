using System;

namespace NewsPublish.API.ApiAdmin.Models.Article
{
    /// <summary>
    /// 返回文章基本信息的DTO
    /// </summary>
    public class ArticleDto
    {
        
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CoverPic { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public bool States { get; set; }
    }
}