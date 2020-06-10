using System.ComponentModel.DataAnnotations;

namespace NewsPublish.API.ApiCommon.Models.Comment
{
    /// <summary>
    /// 添加文章的DTO
    /// </summary>
    
    public class CommentAddDto
    {
        [Display(Name = "评论内容")]
        [Required(ErrorMessage = "{0}这个属性是必填的")]
        [MinLength(10, ErrorMessage = "{0}的最小长度不可低于{1}")]
        public string Content { get; set; }
    }
}