using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiAdmin.Models.Category
{
    /// <summary>
    /// 添加修改分类DTO
    /// </summary>
    public abstract class CategoryAddOrUpdateDto
    {
        [Display(Name = "分类名称")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(20, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Name { get; set; }
        
        [Display(Name = "分类说明")]
        [MaxLength(100, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Remark { get; set; }
    }
}