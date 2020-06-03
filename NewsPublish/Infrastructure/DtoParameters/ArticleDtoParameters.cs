using System;

namespace NewsPublish.Infrastructure.DtoParameters
{
    /// <summary>
    /// 文章查询参数
    /// </summary>
    public class ArticleDtoParameters
    {
        private const int MaxPageSize = 20;
        
        // 模糊查询
        public string Q { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string OrderBy { get; set; } = "CreateTime";
        
        // 精确查询
        public Guid NewsId { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public string TagName { get; set; }
        public Guid TagId { get; set; }

        
    }
}