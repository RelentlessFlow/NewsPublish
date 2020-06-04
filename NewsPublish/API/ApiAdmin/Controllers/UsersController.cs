using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NewsPublish.API.ApiAdmin.Models.User;
using NewsPublish.Authorization.Filter;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.DTO;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// 用户信息（不包含账号）CURD
    /// 过滤器：管理员、授权用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ServiceFilter(typeof(AdminFilter))]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IUserRepositoryExtendAdmin _userRepository;
        private readonly ITokenList _tokenList;

        public UsersController(IUserRepositoryExtendAdmin userRepository, IMapper mapper, IWebHostEnvironment environment, ITokenList tokenList)
        {
            _userRepository = userRepository ??
                              throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
            _environment = environment ??
                                  throw new ArgumentNullException(nameof(environment));
            _tokenList = tokenList;
        }
        
        /// <summary>
        /// 获得所有用户信息（分页）
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>分页的用户信息</returns>
        [HttpGet(Name = nameof(GetUsers))]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromQuery] UserDtoParameters parameters)
        {
            var users = await _userRepository
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
        
        [HttpGet("{userId}", Name = nameof(GetUser))]
        public async Task<ActionResult<User>>
            GetUser(Guid userId)
        {
            var userEntity = await _userRepository.GetUserWithRole(userId);
            if (userEntity == null) return NotFound();
            return Ok(userEntity);
        }
        
        
        
        /// <summary>
        /// 通过用户ID添加用户详细信息
        /// </summary>
        /// <param name="userAddDto">添加用户时候的表单DTO</param>
        /// <returns>新增用户的路由地址</returns>
        [HttpPost]
        public async Task<ActionResult<User>>
            CreateUserForRoles(UserAddDto userAddDto)
        {
            if (!await _userRepository.RoleIsExists(userAddDto.RoleIdGuid)) return NotFound("用户组不存在");

            if (await _userRepository.UserIsExists(userAddDto.NickName))
                return ValidationProblem("这个用户的昵称已经存在了", userAddDto.NickName);

            var entity = _mapper.Map<User>(userAddDto);
            _userRepository.AddUser(entity);
            await _userRepository.SaveAsync();
            var dtoToReturn = _mapper.Map<UserDto>(entity);
            return CreatedAtRoute(nameof(GetUser),
                new {userId = dtoToReturn.Id},
                dtoToReturn);
        }
        
        /// <summary>
        /// 通过用户ID删除一个用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>204状态码</returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var userEntity = await _userRepository.GetUser(userId);
            if (userEntity == null) return NotFound();
            if (userEntity.Avatar != null)
            {
                var fileFolder = Path.Combine(_environment.WebRootPath, $"file_user/{userEntity.Id}");
                MyTools.DeleteDirFile(fileFolder);
            }
            _userRepository.DeleteUser(userEntity);
            await _userRepository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 通过用户ID删除一个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns>204状态码</returns>
        [HttpPut("{userId}")]
        public async Task<ActionResult<UserDto>> UpdateUserForRole(
            Guid userId, UserUpdateDto user)
        {
            var userEntity = await _userRepository.GetUser(userId);
            // 不存在就添加一个
            if (userEntity == null)
            {
                if (!await _userRepository.RoleIsExists(user.RoleIdGuid)) return NotFound("用户组不存在");

                if (await _userRepository.UserIsExists(user.NickName))
                    return ValidationProblem("这个用户的昵称已经存在了", user.NickName);

                var userToAddEntity = _mapper.Map<User>(user);
                userToAddEntity.Id = userId;
                
                _userRepository.AddUser(userToAddEntity);

                await _userRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<UserDto>(userToAddEntity);

                return CreatedAtRoute(nameof(GetUser),
                    new {userId = dtoToReturn.Id}
                    , dtoToReturn);
            }
            
            _mapper.Map(user, userEntity);

            _userRepository.UpdateUser(userEntity);
            await _userRepository.SaveAsync();
            return NoContent();
        }

        
        /// <summary>
        /// 更新用户状态信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId}/state")]
        public async Task<ActionResult<bool>> ChangeUserState(Guid userId)
        {
            var userEntity = await _userRepository.GetUser(userId);
            if (userEntity == null)
            {
                return NotFound();
            }

            var userEntityStates = userEntity.States;
            userEntity.States = !userEntityStates;
            await _userRepository.SaveAsync();
            return Ok();
        }
        
        /// <summary>
        /// 获取用户状态信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/state")]
        public async Task<ActionResult<bool>> GetUserState(Guid userId)
        {
            var userEntity = await _userRepository.GetUser(userId);
            if (userEntity == null)
            {
                return NotFound();
            }

            return Ok(userEntity.States);
        }
            
        

        /// <summary>
        /// 局部修改用户信息，Pacth请求
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{userId}")]
        public async Task<ActionResult> PartiallyUpdateUserForRole(
            Guid userId, JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            var userEntity = await _userRepository.GetUser(userId);
            // 没有就加进去
            if (userEntity == null)
            {
                var userDto = new UserUpdateDto();
                if (!await _userRepository.RoleIsExists(userDto.RoleIdGuid)) return NotFound("用户组不存在");

                var userToAdd = _mapper.Map<User>(userDto);
                userToAdd.Id = userId;

                _userRepository.AddUser(userToAdd);
                await _userRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<UserDto>(userToAdd);
                return CreatedAtRoute(nameof(GetUser),
                    new
                    {
                        userId = dtoToReturn.Id
                    }, dtoToReturn);
            }

            var dtoToPatch = _mapper.Map<UserUpdateDto>(userEntity);

            _mapper.Map(dtoToPatch, userEntity);
            _userRepository.UpdateUser(userEntity);
            await _userRepository.SaveAsync();
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
                    return Url.Link(nameof(GetUsers), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetUsers), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });

                default:
                    return Url.Link(nameof(GetUsers), new
                    {
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.Q
                    });
            }
        }
        
        /// <summary>
        /// REST规范，获取当前支持的请求类型
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,POST,PUT,DELETE,PATCH,OPTIONS");
            return Ok();
        }
        
        /// <summary>
        /// 获取所有在线的用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("online")]
        public IActionResult GetOnlineUser()
        {
            return Ok(_tokenList.GetTokenLists());
        }
    }
}