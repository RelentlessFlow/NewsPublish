using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiAdmin.Models.Banner
{
    /// <summary>
    /// 添加标签的DTO
    /// </summary>
    public class BannerAddDto
    {
        [Display(Name = "跳转链接")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(100, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Url { get; set; }
        [Display(Name = "标记")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(20, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Remark { get; set; }
    }
}