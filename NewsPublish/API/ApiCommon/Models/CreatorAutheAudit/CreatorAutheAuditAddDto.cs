using System;
using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiCommon.Models.CreatorAutheAudit
{
    public class CreatorAutheAuditAddDto
    {
        [Display(Name = "用户ID")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        public Guid UserId { get; set; }
        [Display(Name = "备注内容")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MinLength(10, ErrorMessage = "{0}的最小长度不可低于{1}")]
        public string Remark { get; set; }
    }
}