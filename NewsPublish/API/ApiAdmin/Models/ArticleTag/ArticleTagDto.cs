using System;

namespace NewsPublish.API.ApiAdmin.Models.ArticleTag
{
    /// <summary>
    /// 文章标签DTO
    /// </summary>
    public class ArticleTagDto
    {
        public Guid ArticleId { get; set; }
        public Guid TagId { get; set; }

    }
}