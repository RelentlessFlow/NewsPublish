using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Right;
using NewsPublish.API.ApiAdmin.Models.Role;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.Database.Entities.RoleEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// 角色信息CURD
    /// 过滤器：管理员、授权用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ServiceFilter(typeof(AdminFilter))]
    [ApiController]
    [Route("api/roles")]
    public class RolesController : Controller
    {
        private readonly IUserRepositoryExtendAdmin _userRepository;
        private readonly IMapper _mapper;

        public RolesController(IUserRepositoryExtendAdmin userRepository, IMapper mapper)
        {
            _userRepository = userRepository ??
                              throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }
        
        /// <summary>
        /// 查询角色信息
        /// </summary>
        /// <param name="parameters">查询参数</param>
        /// <returns>角色信息</returns>
        [HttpGet(Name = nameof(GetRoles))]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = await _userRepository.GetRoles();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return Ok(roleDtos);
        }
        
        /// <summary>
        /// 通过ID获取单个角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>一个角色信息</returns>
        [HttpGet("{roleId}", Name = nameof(GetRole))]
        public async Task<ActionResult<Role>> GetRole(Guid roleId)
        {
            var role = await _userRepository.GetRole(roleId);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RoleDto>(role));
        }
        
        /// <summary>
        /// 创建角色信息
        /// </summary>
        /// <param name="role">创建角色信息的DTO</param>
        /// <returns>新建角色信息的路由地址</returns>
        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(RoleAddDto role)
        {
            if (await _userRepository.RoleIsExists(role.Name))
            {
                return ValidationProblem("该角色名对应的角色信息已经存在了！");
            }
            
            var roleEntities = _mapper.Map<Role>(role);
            
            _userRepository.AddRole(roleEntities);
            await _userRepository.SaveAsync();
            
            var returnDto = _mapper.Map<RoleDto>(roleEntities);
            
            return CreatedAtRoute(nameof(GetRole), new { roleId = returnDto.Id },
                returnDto);
        }
        
        /// <summary>
        /// 通过角色ID删除一个角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>204状态吗</returns>
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var roleEntity = await _userRepository.GetRole(roleId);
            if (roleEntity == null)
            {
                return NotFound();
            }
            _userRepository.DeleteRole(roleEntity);
            // await _userRepository.GetUsers(roleId, null);
            // _userRepository.DeleteRole(roleEntity);
            await _userRepository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 通过ID修改角色名
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="roleName">角色名</param>
        /// <returns>204状态码</returns>
        [HttpPut]
        public async Task<ActionResult<RoleDto>> UpdateRole(Guid roleId, string roleName)
        {
            var entity = await _userRepository.GetRole(roleId);
            if (entity == null)
            {
                Role roleToAddEntity = new Role();
                roleToAddEntity.Name = roleName;
                _userRepository.AddRole(roleToAddEntity);
                 await _userRepository.SaveAsync();
                 var returnDto = _mapper.Map<RoleDto>(roleToAddEntity);
                 return CreatedAtRoute(nameof(GetRole), new { roleId = roleToAddEntity.Id },
                     roleToAddEntity);
            }

            entity.Name = roleName;
            await _userRepository.SaveAsync();
            return NoContent();
        }
        
        /// <summary>
        /// 获取所有支持的请求类型 REST规范
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetRoleOptions()
        {
            Response.Headers.Add("Allow","GET,HEAD,POST,DELETE,PUT,PATCH,OPTIONS");
            return Ok();
        }
        
        /// <summary>
        /// 通过角色ID获取角色的全部权限信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色的全部权限信息</returns>
        [HttpGet]
        [Route("{roleId}/right")]
        public async Task<ActionResult<RightDto>> GetRights(Guid roleId)
        {
            var rights = await _userRepository.GetRoleRights(roleId);
            var returnDtos = _mapper.Map<IEnumerable<RightDto>>(rights);
            return Ok(returnDtos);
        }
        
        /// <summary>
        /// 通过角色ID修改角色的权限信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="rightList">添加权限时的权限ID列表</param>
        /// <returns>200状态码和添加的权限</returns>
        [HttpPut]
        [Route("{roleId}/right")]
        public async Task<ActionResult<RightDto>> UpdateRight(Guid roleId, List<Guid> rightList)
        {
            if (!await _userRepository.RoleIsExists(roleId))
            {
                return ValidationProblem("角色不存在");
            }
            var rights = await _userRepository.GetRoleRights();
            var rightIds = rights.Select(x => x.Id).ToList();
            var match = rightList.All(t => rightIds.Any(b => b == t));
            if (!match)
            {
                StringBuilder rightString = new StringBuilder();
                foreach (var right in rights)
                {
                    rightString.Append("     ");
                    rightString.Append(right.Id);
                    rightString.Append("     ");
                    rightString.Append(right.Name);
                    rightString.Append("||");
                }
                return ValidationProblem($"权限信息ID填写错误，权限ID列表如下：{rightString}");
            }

            var entities = await _userRepository.GetRightRoleEntitiesByRoleId(roleId);
            foreach (var entity in entities)
            {
                _userRepository.DeleteRightRole(entity);
            }

            foreach (var rightId in rightList)
            {
                _userRepository.AddRightRole(new RightRole
                {
                    RightId = rightId,
                    RoleId = roleId
                });
            }

            await _userRepository.SaveAsync();
            return Ok(rightList);
        }
    }
}