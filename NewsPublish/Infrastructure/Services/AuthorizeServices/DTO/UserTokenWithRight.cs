using System.Collections.Generic;

namespace NewsPublish.Infrastructure.Services.AuthorizeServices.DTO
{
    /// <summary>
    /// 存储用户的登陆信息（Token信息、角色信息、账号信息）在TokenList
    /// </summary>
    public class UserTokenWithRight
    {
        public List<string> RightName { get; set; } 
        public string Account { get; set; }
        public string Token { get; set; }
    }
}