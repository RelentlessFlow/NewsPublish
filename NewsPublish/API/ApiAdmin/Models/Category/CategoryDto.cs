using System;

namespace NewsPublish.API.ApiAdmin.Models.Category
{
    /// <summary>
    /// 返回分类信息的DTO
    /// </summary>
    public class CategoryDto
    {   
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public string Remark { get; set; }
    }
}