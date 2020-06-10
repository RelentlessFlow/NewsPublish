using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.API.ApiCommon.Models.Comment;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 过滤器：授权
    /// 评论的CRD
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ApiController]
    [Route("api/article/{articleId}/comment")]
    public class CommentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;

        public CommentsController(IMapper mapper, ICommentRepository repository, IArticleRepository articleRepository)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _commentRepository = repository ?? throw new ArgumentException(nameof(repository));
            _articleRepository = articleRepository ?? throw new ArgumentException(nameof(articleRepository));
        }
        
        /// <summary>
        /// 通过文章ID获取文章的全部评论（分页）
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(
            Guid articleId, [FromQuery] CommentDtoParameters parameters)
        {
            var comment = await _commentRepository.GetComments(articleId,parameters);
            
            var previousPageLink = comment.HasNext
                ? CreateUsersResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = comment.HasNext
                ? CreateUsersResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = comment.TotalCount,
                pageSize = comment.PageSize,
                currentPage = comment.CurrentPage,
                totalPages = comment.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            return Ok(comment);
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<Comment>> GetComment(Guid commentId)
        {
            var commentEntity = await _commentRepository.GetComment(commentId);
            if (commentEntity == null)
            {
                return NotFound();
            }

            return commentEntity;
        }
        
        /// <summary>
        /// 新建评论
        /// 过滤器：用户
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="comment">新建评论的DTO</param>
        /// <returns>新建评论的路由地址</returns>
        [ServiceFilter(typeof(UserFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateComment(Guid articleId, CommentAddDto comment)
        {
            if (!await _articleRepository.ArticleIsExists(articleId))
            {
                return NotFound();
            }

            var addToEntity = _mapper.Map<Comment>(comment);
            _commentRepository.AddComments(articleId, addToEntity);
            return CreatedAtRoute(nameof(GetComment), new {commentId = addToEntity.Id}, addToEntity);
        }

        /// <summary>
        /// 通过评论ID删除评论
        /// 过滤器：用户
        /// </summary>
        /// <param name="commentId">评论ID</param>
        /// <returns>204</returns>
        [ServiceFilter(typeof(AdminFilter))]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var commentEntity = await _commentRepository.GetComment(commentId);
            if (commentEntity == null)
            {
                return NotFound();
            }
            _commentRepository.DeleteComment(commentEntity);
            return NoContent();
        }

        private string CreateUsersResourceUri(CommentDtoParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetComments), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetComments), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                default:
                    return Url.Link(nameof(GetComments), new
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