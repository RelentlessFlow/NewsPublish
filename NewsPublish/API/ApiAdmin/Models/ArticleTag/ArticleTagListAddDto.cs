using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiAdmin.Models.ArticleTag
{
    /// <summary>
    /// 文章标签列表
    /// </summary>
    public class ArticleTagListAddDto
    {
        [Display(Name= "标签列表")]
        [Required]
        public List<string> ArticleTags { get; set; }
    }
}