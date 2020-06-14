

using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.API.ApiCommon.Models.ArticleReview;
using NewsPublish.API.ApiCommon.Models.Tag;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AssessServices.DTO;
using NewsPublish.Infrastructure.Services.AssessServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 文章认证控制器 过滤器：认证用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [Route("api_assessor/articleReview")]
    public class ArticleReviewController : ControllerBase
    {    
        private readonly IArticleReviewRepository _articleReviewRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        public ArticleReviewController(IArticleReviewRepository articleReviewRepository,IArticleRepository articleRepository,IMapper mapper)
        {
            _articleReviewRepository = articleReviewRepository ?? throw new ArgumentException(nameof(articleReviewRepository));
            _articleRepository = articleRepository ?? throw new ArgumentException(nameof(articleRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        
       
        
        /// <summary>
        /// 获取所有的创作者认证报表（分页） 过滤器：审查员
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilter(typeof(AssessorFilter))]
        public async Task<ActionResult<IEnumerable<CreatorAutheAuditsDto>>> GetCreatorAutheAuditsList([FromQuery] ArticleReviewAuditDtoParameters parameters)
        {
            var articleReviewAudit = await _articleReviewRepository.GetAllArticleReviewAudit(parameters);
            
            foreach (var article in articleReviewAudit)
            {
                var tag = await _articleRepository.GetArticleAllTags(article.ArticleId);
                IEnumerable<TagDto> tags = _mapper.Map<IEnumerable<TagDto>>(tag);
                article.Tags = tags as List<TagDto>;
            }
            
            foreach (var article in articleReviewAudit)
            {
                var star = await _articleRepository.GetArticleStar(article.ArticleId);
                if (star == null)
                {
                    article.Star = 0;
                }
                else
                {
                    article.Star = star.Count;
                }
            }
            
            var previousPageLink = articleReviewAudit.HasNext
                ? CreatePageListResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = articleReviewAudit.HasNext
                ? CreatePageListResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = articleReviewAudit.TotalCount,
                pageSize = articleReviewAudit.PageSize,
                currentPage = articleReviewAudit.CurrentPage,
                totalPages = articleReviewAudit.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            return Ok(articleReviewAudit);
        }
        
        /// <summary>
        /// 根据表单ID 查询单个表单的详细信息 过滤器：审查员
        /// </summary>
        /// <param name="auditId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{auditId}")]
        [ServiceFilter(typeof(AssessorFilter))]
        public async Task<ActionResult<CreatorAutheAuditsDto>> GetCreatorAutheAudit(Guid auditId)
        {
            var articleReviewAudit  = await _articleReviewRepository.GetArticleReviewAudit(auditId);
            if (articleReviewAudit == null)
            {
                return NotFound();
            }
            return Ok(articleReviewAudit);
        }
        
        /// <summary>
        /// 根据表单ID 查询单个表单的详细信息 过滤器：用户
        /// </summary>
        /// <param name="auditId"></param>
        /// <returns></returns>
        [Route("/api_user/articleReview/{auditId}")]
        [HttpGet]
        [ServiceFilter(typeof(UserFilter))]
        public async Task<ActionResult<CreatorAutheAuditsDto>> GetCreatorAutheAuditByUserId(Guid auditId)
        {
            var articleReviewAudit  = await _articleReviewRepository.GetArticleReviewAudit(auditId);
            if (articleReviewAudit == null)
            {
                return NotFound();
            }
            return Ok(articleReviewAudit);
        }
        
        /// <summary>
        /// 审核文章 过滤器：审查员
        /// </summary>
        /// <param name="auditId"></param>
        /// <param name="editStateDto"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(AssessorFilter))]
        [HttpPut]
        [Route("{auditId}")]
        public async Task<ActionResult<ArticleReviewAudit>> ChangeCreatorAutheAuditState(Guid auditId,ArticleReviewEditStateDto editStateDto)
        {
            var auditEntity  = await _articleReviewRepository.GetArticleReviewAuditEntity(auditId);
            if (auditEntity == null)
            {
                return NotFound();
            }
            
            auditEntity.IsPass = editStateDto.IsPass;
            if (auditEntity.IsPass)
            {
                var article = await _articleRepository.GetArticle(auditEntity.ArticleId);
                article.States = editStateDto.State;
            }
            auditEntity.ReviewRemark = editStateDto.Remark;
            auditEntity.ReviewTime = DateTime.Now;
            auditEntity.ReviewTime = DateTime.Now;
            await _articleReviewRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("{auditId}")]
        [ServiceFilter(typeof(AssessorFilter))]
        public async Task<IActionResult> DeleteAutheAudit(Guid auditId)
        {
            var auditEntity  = await _articleReviewRepository.GetArticleReviewAuditEntity(auditId);
            _articleReviewRepository.DeleteArticleReviewAuditEntity(auditEntity);
             await _articleRepository.SaveAsync();
            return NoContent();
        }

        private string CreatePageListResourceUri(ArticleReviewAuditDtoParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCreatorAutheAuditsList), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCreatorAutheAuditsList), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                default:
                    return Url.Link(nameof(GetCreatorAutheAuditsList), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });
            }
        }
    }
}