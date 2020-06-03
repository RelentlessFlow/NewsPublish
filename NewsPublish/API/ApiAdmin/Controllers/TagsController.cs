using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Role;
using NewsPublish.API.CommenDto.Tag;
using NewsPublish.Authorization.Filter;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Database.Entities.RoleEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// 管理员对标签的CURD操作
    /// 过滤器：管理员、授权用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ServiceFilter(typeof(AdminFilter))]
    [ApiController]
    [Route("api/tags")]
    public class TagsController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public TagsController(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository ??
                                throw new ArgumentNullException(nameof(articleRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

       /// <summary>
       /// 分页查找标签信息
       /// </summary>
       /// <param name="parameters">查询条件</param>
       /// <returns>标签信息</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags([FromQuery]TagDtoParameters parameters)
        {
            var tags = await _articleRepository
                .GetTags(parameters);

            var previousPageLink = tags.HasNext
                ? CreateTagResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = tags.HasNext
                ? CreateTagResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = tags.TotalCount,
                pageSize = tags.PageSize,
                currentPage = tags.CurrentPage,
                totalPages = tags.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));
            var dtoToReturn = _mapper.Map<IEnumerable<TagDto>>(tags);
            return Ok(dtoToReturn);
        }
        
        
        
        /// <summary>
        /// 通过ID获取标签的具体信息
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet("{tagId}", Name = nameof(GetTag))]
        public async Task<ActionResult<Role>> GetTag(Guid tagId)
        {
            var tag = await _articleRepository.GetTag(tagId);

            if (tag == null)
            {
                return NotFound();
            }

            var dtoToReturn = _mapper.Map<TagDto>(tag);
            return Ok(dtoToReturn);
        }
        
        /// <summary>
        /// 创建标签信息
        /// </summary>
        /// <param name="name">标签名</param>
        /// <returns>新标签的路由地址</returns>
        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(string name)
        {
            if (await _articleRepository.TagIsExists(name))
            {
                return ValidationProblem("该角色名对应的角色信息已经存在了！");
            }
            var addToEntity = new Tag
            {
                Name = name
            };

            _articleRepository.AddTag(addToEntity);
            await _articleRepository.SaveAsync();
            var dtoToReturn = _mapper.Map<TagDto>(addToEntity);
            return CreatedAtRoute(nameof(GetTag), new { tagId =  addToEntity.Id},
                dtoToReturn);
        }
        
        /// <summary>
        /// 通过标签ID删除标签
        /// </summary>
        /// <param name="tagId">标签ID</param>
        /// <returns>204状态码</returns>
        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteRole(Guid tagId)
        {
            var tagEntity = await _articleRepository.GetTag(tagId);
            if (tagEntity == null)
            {
                return NotFound();
            }
            _articleRepository.DeleteTag(tagEntity);
            
            await _articleRepository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 通过标签ID修改标签信息
        /// </summary>
        /// <param name="tagId">标签ID</param>
        /// <param name="name">标签名</param>
        /// <returns></returns>
        [HttpPut("{tagId}")]
        public async Task<ActionResult<RoleDto>> UpdateRole(Guid tagId, string name)
        {
            if (name == null)
            {
                return ValidationProblem("您可以修改标签的名称(name)!");
            }
            
            var entity = await _articleRepository.GetTag(tagId);
            if (entity == null)
            {
                if (await _articleRepository.TagIsExists(name))
                {
                    return ValidationProblem("标签名已经存在");
                }
                Tag toAddEntity = new Tag();
                toAddEntity.Name = name;
                _articleRepository.AddTag(toAddEntity);
                 await _articleRepository.SaveAsync();
                 var dtoToReturn = _mapper.Map<TagDto>(toAddEntity);
                 return CreatedAtRoute(nameof(GetTag), new { tagId =  dtoToReturn.Id},
                     dtoToReturn);
            }
            
            entity.Name = name;
            await _articleRepository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 创建分页上下文链接
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string CreateTagResourceUri(TagDtoParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetTag), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetTag), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                default:
                    return Url.Link(nameof(GetTag), new
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