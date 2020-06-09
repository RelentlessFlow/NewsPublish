using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NewsPublish.API.ApiSite.Models.User;
using NewsPublish.Authorization.ConfigurationModel;
using NewsPublish.Authorization.Filter;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiSite.Controllers
{
    /// <summary>
    /// 用户信息查看修改API
    /// 过滤器：授权
    /// </summary>
    [ApiController]
    // [ServiceFilter(typeof(AutheFilter))]
    [Route("api_site/userinfo/{id}")]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<SystemFunctionOptionName> _nameOptions;


        public UserInfoController(IUserRepository repository, IMapper mapper, IOptions<SystemFunctionOptionName> nameOptions)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _nameOptions = nameOptions ?? throw new ArgumentException(nameof(nameOptions));
        }
        
        /// <summary>
        /// 获得所有用户信息（分页）
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>分页的用户信息</returns>
        [HttpGet(Name = nameof(GetUserInfos))]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<User>>> GetUserInfos(
            [FromQuery] UserDtoParameters parameters)
        {
            var users = await _repository
                .GetUsers(parameters);

            var previousPageLink = users.HasNext
                ? CreateUsersResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = users.HasNext
                ? CreateUsersResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = users.TotalCount,
                pageSize = users.PageSize,
                currentPage = users.CurrentPage,
                totalPages = users.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            return Ok(users);
        }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("nickName")]
        public async Task<IActionResult> ChangUserNickName(Guid id ,string nickName)
        {
            if (! await _repository.UserIsExists(id))
            {
                return NotFound();
            }

            if (nickName == null)
            {
                return ValidationProblem(nickName);
            }
            var user = await _repository.GetUser(id);
            user.NickName = nickName;
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// 修改用户头像URL
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avatarUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("avatarUrl")]
        public async Task<IActionResult> ChangUserAvatar(Guid id, string avatarUrl)
        {
            if (! await _repository.UserIsExists(id))
            {
                return NotFound();
            }
            
            if (avatarUrl == null)
            {
                return ValidationProblem(avatarUrl);
            }
            
            var user = await _repository.GetUser(id);
            user.Avatar = avatarUrl;
            await _repository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 修改用户介绍
        /// </summary>
        /// <param name="id"></param>
        /// <param name="introduce"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("introduce")]
        public async Task<IActionResult> ChangUserIntroduce(Guid id, string introduce)
        {
            if (! await _repository.UserIsExists(id))
            {
                return NotFound();
            }
            
            if (introduce == null)
            {
                return ValidationProblem(introduce);
            }
            
            var user = await _repository.GetUser(id);
            user.Introduce = introduce;
            await _repository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 分页上下文
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string CreateUsersResourceUri(UserDtoParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetUserInfos), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetUserInfos), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                default:
                    return Url.Link(nameof(GetUserInfos), new
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