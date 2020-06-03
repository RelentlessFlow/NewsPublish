using System;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.API.ApiAdmin.Models.UserAuthe
{
    /// <summary>
    ///查询用户账号信息的DTO对象
    /// </summary>
    public class UserAutheDto
    {
        public Guid Id { get; set; }
        public string Account { get; set; }
        public UserAuthType AuthType { get; set; }
        public string Credential { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public Guid UserId { get; set; }
    }
    
    
    
}