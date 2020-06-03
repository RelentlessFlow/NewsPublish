using System;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.API.ApiSite.Models.Register
{
    /// <summary>
    /// 用户注册完成返回的DTO对象
    /// </summary>
    public class RegisterReturnDto
    {
        public Guid Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public UserAuthType AccountType { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string Introduction { get; set; }
        public DateTime RegisterTime { get; set; }
    }
}