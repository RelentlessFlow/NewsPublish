

using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.API.ApiCommon.Models.CreatorAutheAudit;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AssessServices.DTO;
using NewsPublish.Infrastructure.Services.AssessServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 创作者认证控制器
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [Route("api_assessor/creatorAuthe")]
    public class CreatorAutheController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICreatorAutheAuditsRepository _auditsRepository;
        public CreatorAutheController(IUserRepository userRepository, ICreatorAutheAuditsRepository auditsRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentException(nameof(IUserRepository));
            _auditsRepository = auditsRepository ?? throw new ArgumentException(nameof(auditsRepository));
        }
        
        /// <summary>
        /// 提交创作者认证信息  过滤器：用户
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api_user/creatorAuthe")]
        [ServiceFilter(typeof(UserFilter))]
        public async Task<ActionResult<CreatorAutheAuditsDto>> RegisterUser(Guid userId,string remark)
        {
            if (userId==Guid.Empty||remark==null||remark=="")
            {
                return ValidationProblem("参数传递异常");
                if (remark.Length<=6)
                {
                    return ValidationProblem("备注长度最低为6");
                }
            }
            if (!await _userRepository.UserIsExists(userId))
            {
                return NotFound();
            }
        
            var addToEntity = new CreatorAutheAudit
            {
                Remark = remark,
                UserId = userId
            };
            _auditsRepository.AddCreatorAutheAudits(addToEntity);
            await _auditsRepository.SaveAsync();
            return CreatedAtRoute((nameof(GetCreatorAutheAudit)),new {auditId = addToEntity.Id},addToEntity);
        }
        
        /// <summary>
        /// 获取所有的创作者认证报表（分页） 过滤器：审查员
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilter(typeof(AssessorFilter))]
        public async Task<ActionResult<IEnumerable<CreatorAutheAuditsDto>>> GetCreatorAutheAuditsList(
            [FromQuery] CreatorAutheAuditsDtoParameters parameters)
        {
            var comment = await _auditsRepository.GetAllCreatorAutheAudits(parameters);
            
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
        
        /// <summary>
        /// 根据表单ID 查询单个表单的详细信息 过滤器：审查员
        /// </summary>
        /// <param name="auditId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api_assessor/creatorAuthe/{auditId}",Name = nameof(GetCreatorAutheAudit))]
        [ServiceFilter(typeof(AssessorFilter))]
        public async Task<ActionResult<CreatorAutheAuditsDto>> GetCreatorAutheAudit(Guid auditId)
        {
            var creatorAutheAudit  = await _auditsRepository.GetCreatorAutheAudit(auditId);
            if (creatorAutheAudit == null)
            {
                return NotFound();
            }
            return Ok(creatorAutheAudit);
        }
        
        /// <summary>
        /// 根据用户ID 查询单个表单的详细信息 过滤器：用户
        /// </summary>
        /// <param name="auditId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api_user/creatorAuthe/{userId}")]
        [ServiceFilter(typeof(UserFilter))]
        public async Task<ActionResult<CreatorAutheAuditsDto>> GetCreatorAutheAuditByUserId(Guid userId)
        {
            var creatorAutheAudit  = await _auditsRepository.GetAllCreatorAutheAudits(new CreatorAutheAuditsDtoParameters
            {
                userId = userId
            });
            if (creatorAutheAudit == null)
            {
                return NotFound();
            }
            return Ok(creatorAutheAudit);
        }
        
        /// <summary>
        /// 获取认证状态
        /// </summary>
        /// <param name="auditId"></param>
        /// <param name="editStateDto"></param>
        /// <returns></returns>
        [Route("/api_assessor/{auditId}/state")]
        [HttpPut]
        public async Task<IActionResult> ChangeCreatorAutheAuditState(Guid auditId,CreatorAutheAuditEditStateDto editStateDto)
        {
            var auditEntity  = await _auditsRepository.GetCreatorAutheAuditEntity(auditId);
            if (auditEntity == null)
            {
                return NotFound();
            }
            
            auditEntity.IsPass = editStateDto.IsPass;
            if (auditEntity.IsPass)
            {
                var user = await _userRepository.GetUser(auditEntity.UserId);
                var role = await _userRepository.GetRole("自媒体");
                user.RoleId = role.Id;
            }

            auditEntity.AuditStatus = true; 
            auditEntity.ReviewRemark = editStateDto.ReviewRemark;
            auditEntity.ReviewTime = DateTime.Now;
            await _auditsRepository.SaveAsync();
            return NoContent();
        }

        private string CreateUsersResourceUri(CreatorAutheAuditsDtoParameters parameters,
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