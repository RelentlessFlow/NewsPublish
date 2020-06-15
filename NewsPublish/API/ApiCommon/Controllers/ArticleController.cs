using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Article;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.API.ApiCommon.Models.Tag;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AssessServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 文章信息CRUD
    /// </summary>
    [ApiController]
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IArticleReviewRepository _articleReviewRepository;

        public ArticleController(
            IArticleRepository repository, 
            IMapper mapper,
            IWebHostEnvironment environment, 
            ICommentRepository commentRepository, 
            IUserRepository userRepository,
            IArticleReviewRepository articleReviewRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _commentRepository = commentRepository ?? throw new ArgumentException(nameof(commentRepository));
            _userRepository = userRepository;
            _articleReviewRepository =
                articleReviewRepository ?? throw new ArgumentException(nameof(articleReviewRepository));
        }
        
        /// <summary>
        /// 获取文章列表 这个是阉割过的谁都能访问到
        /// </summary>
        /// <param name="parameters">查询条件</param>
        /// <returns>文章列表</returns>
        [HttpGet]
        [Route("/api_site/article")]
        public async Task<ActionResult<IEnumerable<ArticleListDto>>> GetAllArticleList([FromQuery] ArticleDtoParameters parameters)
        {
            var articleListData = await this.GetArticleListData(parameters);
            return articleListData;
        }
        
        /// <summary>
        /// 获取文章详细内容
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <returns>文章详细内容</returns>
        [HttpGet]
        [Route("/api_site/article/{articleId}",Name = nameof(GetArticle))]
        public async Task<ActionResult<ArticleDetailDto>> GetArticle(Guid articleId)
        {
            var articles = await _repository.GetArticleDetail(articleId);
            foreach (var article in articles)
            {
                var tag = await _repository.GetArticleAllTags(article.ArticleId);
                IEnumerable<TagDto> tags = _mapper.Map<IEnumerable<TagDto>>(tag);
                article.Tags = tags as List<TagDto>;
                var comments = await _commentRepository.GetComments(article.ArticleId,new CommentDtoParameters());
                foreach (var comment in comments)
                {
                    var commentStar = await _commentRepository.GetCommentStar(comment.Id);
                    if (commentStar != null)
                    {
                        comment.StarCount = commentStar.Count;
                    }
                }
                article.Comments = comments;
                article.CommentsCount = comments.TotalCount;
            }
            
            // 文章点赞数量
            foreach (var article in articles)
            {
                var star = await _repository.GetArticleStar(article.ArticleId);
                if (star == null)
                {
                    article.Star = 0;
                }
                else
                {
                    article.Star = star.Count;
                }
            }
            return Ok(articles);
        }
        
        /// <summary>
        /// 获取全部文章包括未审查的 过滤器：审查管理员
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api_assessor/article")]
        [ServiceFilter(typeof(AutheFilter))]
        [ServiceFilter(typeof(AssessorFilter))]
        public async Task<ActionResult<IEnumerable<ArticleListDto>>> GetArticleList([FromQuery] ArticleDtoParameters parameters)
        {
            var articleListData = await GetArticleListData(parameters, true);
            return articleListData;
        }
        
        /// <summary>
        /// 通过文章ID修改文章状态 过滤器：审查管理员
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <returns></returns>
        [ServiceFilter(typeof(AutheFilter))]
        [ServiceFilter(typeof(AssessorFilter))]
        [HttpPut]
        [Route("/api_assessor/article/{article}/state")]
        public async Task<ActionResult<bool>> ChangeArticleState(Guid articleId)
        {
            var articleEntity = await _repository.GetArticle(articleId);
            if (articleEntity == null)
            {
                return NotFound();
            }

            var state = articleEntity.States;
            articleEntity.States = !state;
            await _repository.SaveAsync();
            return Ok(articleEntity.States);
        }
        
        /// <summary>
        /// 查看所有审核没有通过的文章    过滤器：创作者
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(AutheFilter))]
        [ServiceFilter(typeof(CreatorFilter))]
        [HttpGet]
        [Route("/api_creator/articleFailed")]
        public async Task<ActionResult<IEnumerable<ArticleListDto>>> GetArticleListByCreator(Guid userId,[FromQuery] ArticleDtoParameters parameters)
        {
            if (!await _userRepository.UserIsExists(userId))
            {
                return NotFound();
            }
            
            parameters.isPass = false;
            parameters.UserId = userId;
            var articleListData = await GetArticleListData(parameters, true);
            return articleListData;
        }
        
        /// <summary>
        /// 查看文章是否通过    过滤器：创作者
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(AutheFilter))]
        [ServiceFilter(typeof(CreatorFilter))]
        [HttpGet]
        [Route("/api_creator/article/{article}/state")]
        public async Task<ActionResult<bool>> GetArticleState(Guid userId)
        {
            var articleEntity = await _repository.GetArticle(userId);
            if (articleEntity == null)
            {
                return NotFound();
            }

            return Ok(articleEntity.States);
        }



        /// <summary>
        /// 新建一篇文章    过滤器：创作者
        /// </summary>
        /// <param name="article">创建文章的DTO</param>
        /// <returns>新建文章的路由地址</returns>
        // [ServiceFilter(typeof(AutheFilter))]
        // [ServiceFilter(typeof(CreatorFilter))]
        [HttpPost]
        [Route("/api_creator/article")]
        public async Task<ActionResult<ArticleDto>> CreateArticle(ArticleAddDto article)
        {
            if (await _repository.ArticleIsExists(article.Title))
            {
                return ValidationProblem("新闻已经发表过了！");
            }
            
            var addToDto = _mapper.Map<Article>(article);
            _repository.AddArticle(article.CategoryId,article.UserId,addToDto);

            var articleReviewAuditAddToEntity = new ArticleReviewAudit
            {
                ArticleId = addToDto.Id,
                AuditStatus = false,
                CreateTime = DateTime.Now
            };
            
            _articleReviewRepository.AddArticleReviewAudits(articleReviewAuditAddToEntity);
            
            var dtoToReturn = _mapper.Map<ArticleDto>(addToDto);
            await _repository.SaveAsync();
            return CreatedAtRoute(nameof(GetArticle), new {articleId = addToDto.Id}, new
            {
                dtoToReturn,
                articleReviewAuditAddToEntity.AuditStatus,
                articleReviewAuditAddToEntity.Id,
                articleReviewAuditAddToEntity.CreateTime
            });
        }
        
        /// <summary>
        /// 删除文章
        /// 过滤器：创作者
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns>204状态码</returns>
        [ServiceFilter(typeof(AutheFilter))]
        [ServiceFilter(typeof(CreatorFilter))]
        [HttpDelete]
        [Route("/api_creator/article/{articleId}")]
        public async Task<IActionResult> DeleteArticle(Guid articleId)
        {
            var articleEntities = await _repository.GetArticle(articleId);
            if (articleEntities == null)
            {
                return NotFound();
            }

            _repository.DeleteArticle(articleEntities);
            await _repository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 更新文章
        /// 过滤器：创作者
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="article">更新文章的DTO</param>
        /// <returns></returns>
        [ServiceFilter(typeof(AutheFilter))]
        [ServiceFilter(typeof(CreatorFilter))]
        [HttpPut]
        [Route("/api_creator/article/{articleId}")]
        public async Task<ActionResult<Article>> UpdateArticle(Guid articleId ,ArticleUpdateDto article)
        {
            var entities = await _repository.GetArticle(articleId);
            if (entities == null)
            {
                return NotFound();
            }

            _mapper.Map(article, entities);
            await _repository.SaveAsync();
            return NoContent();
        }
        
        [ServiceFilter(typeof(AdminFilter))]
        [Route("/api/article/{articleId}/state")]
        [HttpPut]
        public async Task<IActionResult> ChangeArticleStateByAdmin(Guid articleId)
        {
            var article = await _repository.GetArticle(articleId);
            if (article == null)
            {
                return NotFound();
            }

            var flag = !article.States;
            article.States = flag;
            await _repository.SaveAsync();
            return NoContent();
        }
        
        

        /// <summary>
        /// 通用查询文章方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="getDisabledArticle"></param>
        /// <returns></returns>
        private async Task<ActionResult<IEnumerable<ArticleListDto>>> GetArticleListData(ArticleDtoParameters parameters, bool getDisabledArticle = false)
        {
            var articles = await _repository.GetArticles(parameters,getDisabledArticle);

            var paginationMetadata = new
            {
                totalCount = articles.TotalCount,
                pageSize = articles.PageSize,
                currentPage = articles.CurrentPage,
                totalPages = articles.TotalPages,
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
            return Ok(articles);
        }

    }
}