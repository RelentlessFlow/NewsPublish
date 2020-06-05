using System;

namespace NewsPublish.Database.Entities.ArticleEntities
{
    public class Star
    {
        public Guid Id { get; set; }
        public Guid StartId { get; set; }
        public StarType Type { get; set; }
        public byte Count { get; set; }
    }

    public enum StarType
    {
        文章 = 1,
        评论 = 2
    }
}