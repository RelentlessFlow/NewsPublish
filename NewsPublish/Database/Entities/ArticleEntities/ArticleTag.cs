using System;

namespace NewsPublish.Database.Entities.ArticleEntities
{
    public class ArticleTag
    {
        public Article Article { get; set; }
        public Tag Tag { get; set; }
        public Guid ArticleId { get; set; }
        public Guid TagId { get; set; }
    }
}