using System;

namespace NewsPublish.Infrastructure.Services.CommonServices.DTO
{
    public class CommentListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public byte StarCount { get; set; }
        
    }
}