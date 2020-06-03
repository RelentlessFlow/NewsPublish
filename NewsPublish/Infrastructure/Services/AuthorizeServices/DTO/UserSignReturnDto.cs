using System;

namespace NewsPublish.Infrastructure.Services.AuthorizeServices.DTO
{
    public class UserSignReturnDto
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}