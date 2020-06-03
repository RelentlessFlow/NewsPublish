using System;

namespace NewsPublish.Infrastructure.DtoParameters
{
    /// <summary>
    /// 用户查询参数
    /// </summary>
    public class UserDtoParameters
    {
        private const int MaxPageSize = 20;
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Q { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string OrderBy { get; set; }
    }
}