using System;
using System.Collections.Generic;
using NewsPublish.API.ApiCommon.Models.Tag;

namespace NewsPublish.Infrastructure.Services.CommonServices.DTO
{
    /// <summary>
    /// 文章列表DTO
    /// </summary>
    public class ArticleListDto
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
        public DateTime ModifyTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool State { get; set; }
    }
}