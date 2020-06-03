using System;

namespace NewsPublish.Database.Entities.WebEntities
{
    /**
     * 广告位 EF数据实体
     */
    public class Banner
    {
        public Guid Id { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
    }
}