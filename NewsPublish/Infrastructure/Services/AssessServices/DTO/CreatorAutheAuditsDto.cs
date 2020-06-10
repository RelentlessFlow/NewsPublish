using System;

namespace NewsPublish.Infrastructure.Services.AssessServices.DTO
{
    public class CreatorAutheAuditsDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Introduction { get; set; }
        public string Remark { get; set; }
        public bool IsPass { get; set; }
        public bool AuditStatus { get; set; }
        public Guid CreatorAutheAuditId { get; set; }
        
        public DateTime CreateTime { get; set; }
    }
}