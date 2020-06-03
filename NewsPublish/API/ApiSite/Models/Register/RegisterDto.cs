using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.API.ApiSite.Models.Register
{
    /// <summary>
    /// 注册用户时的DTO
    /// </summary>
    public class RegisterDto
    {
        [Display(Name = "昵称")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(20, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string NickName { get; set; }
        [Display(Name = "介绍")]
        [MaxLength(100, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Introduce { get; set; }
        
        [Display(Name = "账号")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(20, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Account { get; set; }
        
        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(20, ErrorMessage = "{0}的最大长度不可超过{1}位")]
        [MinLength(8,ErrorMessage = "{0}的最小长度不可低于{1}位")]
        public string Password { get; set; }
        
        public UserAuthType AuthType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Regex regEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
            Regex regPhone = new Regex(
                @"((^13[0-9]{1}[0-9]{8}|^15[0-9]{1}[0-9]{8}|^14[0-9]{1}[0-9]{8}|^16[0-9]{1}[0-9]{8}|^17[0-9]{1}[0-9]{8}|^18[0-9]{1}[0-9]{8}|^19[0-9]{1}[0-9]{8})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)");
            if (AuthType == UserAuthType.手机号)
            {
                if (!regPhone.IsMatch(Account))
                {
                    yield return new ValidationResult(
                        "格式错误，请检查手机号格式");
                }
            }
            else if (AuthType == UserAuthType.邮箱)
            {
                if (!regEmail.IsMatch(Account))
                {
                    yield return new ValidationResult(
                        "格式错误，请检查手机号格式");
                }
            }
            else
            {
                yield return new ValidationResult(
                    "格式错误，请检查账号格式");
            }

            yield return ValidationResult.Success;
        }
    }
}