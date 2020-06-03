using System;

namespace NewsPublish.API.CommenDto.Tag
{
    /// <summary>
    /// 返回标签的DTO
    /// </summary>
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        // 这玩意是用来排序的
        public DateTime CreateTime { get; set; }
    }
}