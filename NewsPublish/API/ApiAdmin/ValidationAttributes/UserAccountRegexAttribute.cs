using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using NewsPublish.API.ApiAdmin.Models.UserAuthe;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.API.ApiAdmin.ValidationAttributes
{
    /// <summary>
    /// 用户账号的复杂验证，实现了ValidationAttribute的IsValid方法
    /// </summary>
    public class UserAccountRegexAttribute  : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dto = (UserAutheAddOrUpdateDto)validationContext.ObjectInstance;
            
            if (!dto.AuthType.Equals(UserAuthType.手机号) || dto.AuthType.Equals(UserAuthType.邮箱))
            {
                return new ValidationResult(
                    $"验证类型为空或者填写不正确，可验证类型：" +
                    $"{UserAuthType.手机号},{UserAuthType.邮箱}、" +
                    $"参考格式{nameof(UserAuthType)}:1," +
                    $"{nameof(UserAuthType)}:2"
                    );
            }
            Regex regEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
            Regex regPhone = new Regex(@"((^13[0-9]{1}[0-9]{8}|^15[0-9]{1}[0-9]{8}|^14[0-9]{1}[0-9]{8}|^16[0-9]{1}[0-9]{8}|^17[0-9]{1}[0-9]{8}|^18[0-9]{1}[0-9]{8}|^19[0-9]{1}[0-9]{8})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)");
            if (dto.AuthType == UserAuthType.手机号)
            {
                if (!regPhone.IsMatch(dto.Account))
                {
                    return new ValidationResult(
                        "格式错误，请检查手机号格式");
                }
            }
            else if (dto.AuthType == UserAuthType.邮箱)
            {
                if (!regEmail.IsMatch(dto.Account))
                {
                    return new ValidationResult(
                        "格式错误，请检查手机号格式");
                }
            }
            else
            {
                return new ValidationResult(
                    "格式错误，请检查账号格式");
            }
            return ValidationResult.Success;
        }
    }
}