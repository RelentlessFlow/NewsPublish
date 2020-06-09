using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Right;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// 权限信息查询
    /// 过滤器：管理员、授权用户
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(AutheFilter))]
    [ServiceFilter(typeof(AdminFilter))]
    [Route("api/right")]
    public class RightController : ControllerBase
    {
        private readonly IUserRepositoryExtendAdmin _userRepository;
        private readonly IMapper _mapper;
        public RightController(IUserRepositoryExtendAdmin repository, IMapper mapper)
        {
            _userRepository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        
        /// <summary>
        /// 获取全部权限信息或角色的所有权限
        /// </summary>
        /// <param name="roleId">角色的ID</param>
        /// <returns>权限信息</returns>
        [HttpGet]
        public async Task<ActionResult<RightDto>> GetRights(Guid roleId = new Guid())
        {
            var rights = await _userRepository.GetRoleRights(roleId);
            var returnDtos = _mapper.Map<IEnumerable<RightDto>>(rights);
            return Ok(returnDtos);
        }


    }
}