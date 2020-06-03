using System;

namespace NewsPublish.API.ApiSite.Models.User
{
    /// <summary>
    /// 用户信息返回DTO
    /// </summary>
    public class UserInfoDto
    {
        public Guid Id { get; set; }
        // 昵称
        public string NickName { get; set; }
        // 头像URL
        public string Avatar { get; set; }
        public string Introduce { get; set; }
        public bool isCreator { get; set; }
    }
}