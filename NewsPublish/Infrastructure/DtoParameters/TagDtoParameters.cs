namespace NewsPublish.Infrastructure.DtoParameters
{
    public class TagDtoParameters
    {
        /// <summary>
        /// 标签查询参数
        /// </summary>
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
        public string Name { get; set; }
    }
}