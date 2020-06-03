using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Article;
using NewsPublish.API.CommenDto.Tag;
using NewsPublish.Authorization.Filter;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 过滤器：授权用户
    /// 文章信息CRUD
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ApiController]
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public ArticleController(IArticleRepository repository, IMapper mapper,IWebHostEnvironment environment, ICommentRepository commentRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _commentRepository = commentRepository ?? throw new ArgumentException(nameof(commentRepository));
        }
        
        /// <summary>
        /// 过滤器：审查管理员
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        // 这个是全的
        [HttpGet]
        [ServiceFilter(typeof(AssessorFilter))]
        public async Task<ActionResult<ArticleListDto>> GetArticleList([FromQuery] ArticleDtoParameters parameters)
        {
            var articleListData = await this.GetArticleListData(parameters, true);
            return articleListData;
        }
        
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="parameters">查询条件</param>
        /// <returns>文章列表</returns>
        // 这个是阉割过的
        [HttpGet]
        [Route("/api_site/article")]
        public async Task<ActionResult<ArticleListDto>> GetAllArticleList([FromQuery] ArticleDtoParameters parameters)
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
        [Route("{articleId}")]
        public async Task<ActionResult<ArticleDetailDto>> GetArticle(Guid articleId)
        {
            var articles = await _repository.GetArticleDetail(articleId);
            foreach (var article in articles)
            {
                var tag = await _repository.GetArticleAllTags(article.ArticleId);
                IEnumerable<TagDto> tags = _mapper.Map<IEnumerable<TagDto>>(tag);
                article.Tags = tags as List<TagDto>;
                var comments = await _commentRepository.GetComments(article.ArticleId,new CommentDtoParameters());
                if (comments != null)
                {
                    article.Comments = comments;
                }
                article.CommentsCount = comments.TotalCount;
            }
            return Ok(articles);
        }
        
        
        /// <summary>
        /// 过滤器：创作者
        /// 查看文章是否通过
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilter(typeof(CreatorFilter))]
        [Route("{article}/state")]
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
        /// 过滤器：审查管理员
        /// 通过文章ID修改文章状态
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <returns></returns>
        [HttpPut]
        [ServiceFilter(typeof(AssessorFilter))]
        [Route("{article}/state")]
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
        /// 过滤器：创作者
        /// 新建一篇文章
        /// </summary>
        /// <param name="article">创建文章的DTO</param>
        /// <returns>新建文章的路由地址</returns>
        [HttpPost]
        [ServiceFilter(typeof(CreatorFilter))]
        public async Task<ActionResult<ArticleDto>> CreateArticle(ArticleAddDto article)
        {
            if (await _repository.ArticleIsExists(article.Title))
            {
                return ValidationProblem("新闻已经发表过了！");
            }
            
            var addToDto = _mapper.Map<Article>(article);
            _repository.AddArticle(article.CategoryId,article.UserId,addToDto);

            var dtoToReturn = _mapper.Map<ArticleDto>(addToDto);
            await _repository.SaveAsync();
            return CreatedAtRoute(nameof(GetArticle), new {articleId = addToDto.Id}, dtoToReturn);
        }
        
        /// <summary>
        /// 过滤器：创作者
        /// 删除文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns>204状态码</returns>
        [ServiceFilter(typeof(CreatorFilter))]
        [HttpDelete("{articleId}")]
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
        /// 过滤器：创作者
        /// 更新文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="article">更新文章的DTO</param>
        /// <returns></returns>
        [ServiceFilter(typeof(CreatorFilter))]
        [HttpPut("{articleId}")]
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
        

        private string CreateArticleResourceUri(ArticleDtoParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetArticle), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetArticle), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                default:
                    return Url.Link(nameof(GetArticle), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });
            }
        }
        
        /// <summary>
        /// 查询文章方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="getDisabledArticle"></param>
        /// <returns></returns>
        private async Task<ActionResult<ArticleListDto>> GetArticleListData(ArticleDtoParameters parameters, bool getDisabledArticle = false)
        {
            var articles = await _repository.GetArticles(parameters,getDisabledArticle);
            foreach (var article in articles)
            {
                var tag = await _repository.GetArticleAllTags(article.ArticleId);
                IEnumerable<TagDto> tags = _mapper.Map<IEnumerable<TagDto>>(tag);
                article.Tags = tags as List<TagDto>;
            }

            return Ok(articles);
        }

    }
}