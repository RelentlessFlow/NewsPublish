using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiSite.Controllers
{
    [ApiController]
    [Route("/api_site/star")]
    public class StarController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;

        public StarController(IArticleRepository articleRepository,ICommentRepository commentRepository)
        {
            _articleRepository = articleRepository ?? throw new ArgumentException(nameof(articleRepository));
            _commentRepository = commentRepository ?? throw new ArgumentException(nameof(commentRepository));
        }

        [HttpGet]
        [Route("article/{articleId}")]
        public async Task<ActionResult<Star>> GetArticleStar(Guid articleId)
        {
            if (! await _articleRepository.ArticleIsExists(articleId))
            {
                return NotFound();
            }

            return Ok(await _articleRepository.GetArticleStar(articleId));
        }

        [HttpGet]
        [Route("comment/{commentId}")]
        public async Task<ActionResult<Star>> GetCommentStar(Guid commentId)
        {
            if (! await _commentRepository.CommentIsExists(commentId))
            {
                return NotFound();
            }

            return Ok(await _commentRepository.GetCommentStar(commentId));
        }
        
        [HttpPost]
        [Route("article/{articleId}")]
        public async Task<IActionResult> AddArticleStar(Guid articleId)
        {
            if (! await _articleRepository.ArticleIsExists(articleId))
            {
                return NotFound();
            }
            _articleRepository.AddArticleStar(articleId);
            await _articleRepository.SaveAsync();
            return NoContent();
        }
        
        [HttpPost]
        [Route("comment/{commentId}")]
        public async Task<IActionResult> AddCommentStar(Guid commentId)
        {
            if (! await _commentRepository.CommentIsExists(commentId))
            {
                return NotFound();
            }
            _commentRepository.AddCommentStar(commentId);
            await _commentRepository.SaveAsync();
            return NoContent();
        }
    }
}