using System;
using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiAdmin.Models.Article
{
    /// <summary>
    /// 更新文章信息的DTO
    /// </summary>
    public class ArticleUpdateDto
    {
        [Display(Name = "文章标题")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(100, ErrorMessage = "{0}的最大长度不可超过{1}")]
        [MinLength(5, ErrorMessage = "{0}的最小长度不可低于{1}")]
        public string Title { get; set; }

        [Display(Name = "文章封面URL")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        public string CoverPic { get; set; }
        
        [Display(Name = "文章内容")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MaxLength(200, ErrorMessage = "{0}的最大长度不可超过{1}")]
        public string Content { get; set; }

        [Display(Name = "分类ID")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        public Guid CategoryId { get; set; }
    }
}