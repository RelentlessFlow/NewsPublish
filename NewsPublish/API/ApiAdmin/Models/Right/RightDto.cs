using System;

namespace NewsPublish.API.ApiAdmin.Models.Right
{
    /// <summary>
    /// 返回权限信息时所用的DTO对象
    /// </summary>
    public class RightDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}