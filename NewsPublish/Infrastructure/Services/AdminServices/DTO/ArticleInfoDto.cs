using System;

namespace NewsPublish.Infrastructure.Services.AdminServices.DTO
{
    /// <summary>
    /// 从数据库出来查询的详细信息DTO
    /// </summary>
    public class ArticleInfoDto
    {
        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryRemark { get; set; }
        public string NewsTitle { get; set; }
        public string CoverPic { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public bool States { get; set; }
    }
}