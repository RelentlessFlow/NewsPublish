using System;
using System.Collections.Generic;

namespace NewsPublish.Database.Entities.ArticleEntities
{
    /// <summary>
    /// 分类表
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        // 外键
        public ICollection<Article> Articles{ get; set; }
    }
}