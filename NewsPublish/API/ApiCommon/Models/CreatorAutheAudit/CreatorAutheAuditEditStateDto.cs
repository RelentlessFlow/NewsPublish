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
        public string ReturnRemark { get; set; }
        
        [Display(Name = "角色ID")]
        public Guid RoleId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsPass)
            {
                if (RoleId == Guid.Empty)
                {
                    yield return new ValidationResult(
                        "如果用户的审查通过了，请为他分配合适的角色ID");
                }
            }
            yield return ValidationResult.Success;
        }
    }
}