namespace NewsPublish.Infrastructure.DtoParameters
{
    /// <summary>
    /// 评论查询参数
    /// </summary>
    public class CommentDtoParameters
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
    }
}