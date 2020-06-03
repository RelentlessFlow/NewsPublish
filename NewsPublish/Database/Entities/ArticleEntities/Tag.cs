using System;
using System.Collections.Generic;

namespace NewsPublish.Database.Entities.ArticleEntities
{
    /// <summary>
    /// 标签表
    /// </summary>
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        // 这玩意是用来排序的
        public DateTime CreateTime { get; set; }
        
        public List<ArticleTag> ArticleTags { get; set; }
    }
}