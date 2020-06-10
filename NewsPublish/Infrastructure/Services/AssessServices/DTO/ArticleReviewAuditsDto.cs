using System;
using System.Collections.Generic;
using NewsPublish.API.ApiCommon.Models.Tag;

namespace NewsPublish.Infrastructure.Services.AssessServices.DTO
{
    public class ArticleReviewAuditsDto
    {
        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Introduction { get; set; }
        
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryRemark { get; set; }
        
        public List<TagDto> Tags { get; set; }
        public string AticleTitle { get; set; }
        public string CoverPic { get; set; }
        public byte Star { get; set; }
        public DateTime ArticleModifyTime { get; set; }
        public DateTime ArticleCreateTime { get; set; }
        public bool State { get; set; }
        public bool IsPass { get; set; }
        public bool AuditStatus { get; set; }
        public Guid UserReviewAuditId { get; set; }
        public DateTime AuditCreateTime { get; set; }
        public DateTime AuditModifyTime { get; set; }
        public string ReviewRemark { get; set; }
    }
}