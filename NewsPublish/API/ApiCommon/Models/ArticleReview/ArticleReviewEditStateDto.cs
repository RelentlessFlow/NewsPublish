using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiCommon.Models.ArticleReview
{
    public class ArticleReviewEditStateDto
    {
        [Display(Name = "是否认证通过")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        public bool IsPass { get; set; }
        
        
        [Display(Name = "是否认证通过的备注")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        public string Remark { get; set; }
        
        [Display(Name = "文章状态")]
        public bool State { get; set; }
    }
}