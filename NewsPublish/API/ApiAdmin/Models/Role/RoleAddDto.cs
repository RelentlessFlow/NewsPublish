using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiAdmin.Models.Role
{
    /// <summary>
    /// 添加角色用的DTO对象
    /// </summary>
    public class RoleAddDto
    {
        [Display(Name = "角色名")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(20, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Name { get; set; }
    }
}