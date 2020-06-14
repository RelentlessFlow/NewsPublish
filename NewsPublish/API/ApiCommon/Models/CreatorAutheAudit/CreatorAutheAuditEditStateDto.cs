using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiCommon.Models.CreatorAutheAudit
{
    public class CreatorAutheAuditEditStateDto
    {
        [Display(Name = "是否认证通过")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        public bool IsPass { get; set; }
        [Display(Name = "是否认证通过的备注")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        public string ReviewRemark { get; set; }

    }
}