using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiSite.Controllers
{
    [Route("api_site/userInfoCollection")]
    public class UserInfoCollection : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        public UserInfoCollection(IUserRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        /// <summary>
        /// 获得所有用户信息（分页）
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>分页的用户信息</returns>
        [HttpGet(Name = nameof(GetUserInfos))]
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