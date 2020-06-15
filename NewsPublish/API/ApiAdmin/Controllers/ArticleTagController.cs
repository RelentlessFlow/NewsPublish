using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.ArticleTag;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.API.ApiCommon.Models.Tag;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// 文章标签信息CURD
    /// 部分创作者功能和管理员功能重叠
    /// 过滤器：管理员、创作者、授权用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ApiController]
    [Route("api/article/{articleId}/tag")]
    public class ArticleTagController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;
        
        public ArticleTagController(IArticleRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        
        /// <summary>
        /// 通过文章ID获取全部标签
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <returns>文章的所有标签</returns>
        // [ServiceFilter(typeof(AdminFilter))]
        [HttpGet(Name = nameof(GetArticleTag))]
        public async Task<ActionResult<IEnumerable<Tag>>> GetArticleTag(Guid articleId)
        {
            var articleAllTags = await _repository.GetArticleAllTags(articleId);
            return Ok(_mapper.Map<IEnumerable<TagDto>>(articleAllTags));
        }
        
        /// <summary>
        /// 通过文章ID添加标签 给创作者和管理员使用的
        /// </summary>
        /// <param name="articleId">文章ID</param>
        /// <param name="list">添加文章标签的DTO</param>
        /// <returns>返回新文章标签的路由信息</returns>
        [Route("/api/article/{articleId}/tag")]
        [Route("/api_creator/article/{articleId}/tag")]
        
        [HttpPost]
        public async Task<IActionResult> CreateArticleTags(Guid articleId,ArticleTagListAddDto list)
        {
            if (!await _repository.ArticleIsExists(articleId))
            {
                return NotFound();
            }

            if (!list.ArticleTags.Any())
            {
                return ValidationProblem("填写标签为空！");
            }
            List<ArticleTagDto> articleTagDtos = new List<ArticleTagDto>();
            List<TagDto> tagDtos = new List<TagDto>();
            foreach (var name in list.ArticleTags)
            {
                var tagEntity = await _repository.GetTag(name);
                if (tagEntity == null)
                {
                    tagEntity = new Tag
                    {
                        Name = name
                    };
                    _repository.AddTag(tagEntity);
                    var tagDto = _mapper.Map<TagDto>(tagEntity);
                    tagDtos.Add(tagDto);
                    
                }
                var articleTagToAdd = new ArticleTag()
                {
                    ArticleId = articleId,
                    TagId = tagEntity.Id
                };
                _repository.AddArticleTag(articleTagToAdd);
                await _repository.SaveAsync();
                
                var articleTagDto = _mapper.Map<ArticleTagDto>(articleTagToAdd);
                articleTagDtos.Add(articleTagDto);
            }

            return CreatedAtRoute(nameof(GetArticleTag), new {articleId = articleId}, new
            {
                articleTagDtos,tagDtos
            });
        }
    }
}