using System;
using System.Collections.Generic;
using NewsPublish.API.CommenDto.Tag;
using NewsPublish.Infrastructure.Helpers;

namespace NewsPublish.Infrastructure.Services.CommonServices.DTO
{
    /// <summary>
    /// 文章的详细信息返回DTO
    /// </summary>
    public class ArticleDetailDto
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
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public string CoverPic { get; set; }
        public DateTime ModifyTime { get; set; }
        public DateTime CreateTime { get; set; }
        public PagedList<CommentListDto> Comments { get; set; }
        public int CommentsCount { get; set; }
    }
}