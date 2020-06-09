using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.UserAuthe;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// 通过用户ID对用户账号信息进行CURD操作
    /// 过滤器：管理员、授权用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ServiceFilter(typeof(AdminFilter))]
    [ApiController]
    [Route("api/users/{userId}/userAuthes")]
    public class UserAuthesController : ControllerBase
    {
        private readonly IUserRepositoryExtendAdmin _userRepository;
        private readonly IMapper _mapper;

        public UserAuthesController(IUserRepositoryExtendAdmin userRepository,IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 通过用户ID获取用户的所有账号
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户的所有账号</returns>
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEquatable<UserAuthe>>> GetUserAuthes(Guid userId)
        {
            if (! await _userRepository.UserIsExists(userId))
            {
                return NotFound();
            }

            var userAuthesEntity = 
                await _userRepository.GetUserAuthes(userId);

            var returnDto =
                _mapper.Map<IEnumerable<UserAutheDto>>(userAuthesEntity);
            
            return Ok(returnDto);
        }

        /// <summary>
        /// 通过用户ID和账号ID获取账号详细信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userAutheId">账号ID（数据库主键）</param>
        /// <returns></returns>
        [HttpGet("{userAutheId}", Name = nameof(GetUserAuthe))]
        public async Task<ActionResult<UserAuthe>> GetUserAuthe(Guid userId, Guid userAutheId)
        {
            if (!await _userRepository.UserIsExists(userId))
            {
                return NotFound();
            }
        
            var userAuthe = await _userRepository.GetUserAuthe(userId, userAutheId);
            if (userAuthe ==null)
            {
                return NotFound();
            }
        
            var returnDto = _mapper.Map<UserAutheDto>(userAuthe);
            return Ok(returnDto);
        }
        
        /// <summary>
        /// 通过用户ID创建用户账号信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="autheAddDto">添加用户的账号的DTO</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUserAuthe(Guid userId,UserAutheAddDto autheAddDto)
        {
            if (!await _userRepository.UserIsExists(userId))
            {
                return NotFound();
            }

            if (await _userRepository.UserAutheIsExists(autheAddDto.Account))
            {
                return ValidationProblem("该验证信息已经存在！");
            }
            
            var entity = _mapper.Map<UserAuthe>(autheAddDto);
            _userRepository.AddUserAuthe(userId,entity);
            await _userRepository.SaveAsync();
            var dtoToReturn = _mapper.Map<UserAutheDto>(entity);
            return CreatedAtRoute(nameof(GetUserAuthe), new {
                    userId = dtoToReturn.UserId,
                    userAutheId  = dtoToReturn.Id
            }, dtoToReturn);
        }
        
        /// <summary>
        /// 通过用户ID和用户账号ID主键更新账号信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="autheId">账号ID</param>
        /// <param name="autheUpdateDto">更新时的DTO</param>
        /// <returns></returns>
        [HttpPut("{autheId}")]
        public async Task<ActionResult<UserAutheDto>> UpdateUserForRole(
            Guid userId, Guid autheId, UserAutheUpdateDto autheUpdateDto)
        {
            var autheEntity = await _userRepository.GetUserAuthe(userId, autheId);
            if (autheEntity == null)
            {
                if (!await _userRepository.UserIsExists(userId))
                {
                    return NotFound("用户不存在");
                }

                if (!await _userRepository.UserAutheIsExists(autheUpdateDto.Credential))
                {
                    return NotFound("验证信息已经存在了！");
                }
                
                var autheToAddEntity = _mapper.Map<UserAuthe>(autheUpdateDto);
                autheToAddEntity.UserId = userId;
                _userRepository.AddUserAuthe(userId,autheToAddEntity);
                await _userRepository.SaveAsync();
                var dtoToReturn = _mapper.Map<UserAutheDto>(autheToAddEntity);
                return CreatedAtRoute(nameof(GetUserAuthe),
                    new {userId = dtoToReturn.UserId, userAutheId = dtoToReturn.Id}
                    , dtoToReturn
                );
            }
            var entity = _mapper.Map(autheUpdateDto, autheEntity);
            entity.ModifyTime = DateTime.Now;
            await _userRepository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 通过用户ID和用户账号ID主键删除账号信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="autheId"></param>
        /// <returns></returns>
        [HttpDelete("{autheId}")]
        public async Task<IActionResult> DeleteUserAuthe(Guid userId, Guid autheId)
        {
            var autheEntity = await _userRepository.GetUserAuthe(userId,autheId);
            if (autheEntity == null)
            {
                return NotFound();
            }
            _userRepository.DeleteUserAuthe(autheEntity);
            await _userRepository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 获取当前所支持的请求类型 REST规范
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetRoleOptions()
        {
            Response.Headers.Add("Allow","GET,HEAD,,POST,PUT,DELETE,OPTIONS");
            return Ok();
        }
    }
}