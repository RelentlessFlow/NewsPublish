using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAdmin.Models.Login;
using NewsPublish.Authorization.Filter;
using NewsPublish.Infrastructure.Services;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;
using NewsPublish.Infrastructure.Services.AuthorizeServices.DTO;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Interface;

namespace NewsPublish.Authorization.Controllers
{
    /// <summary>
    /// 授权控制器，主要完成用户的授权管理
    /// </summary>
    [ApiController]
    [Route("/api/attribute", Name = nameof(AttributeController))]
    public class AttributeController : ControllerBase
    {
        private readonly IUserRepositoryExtendAdmin _userRepository;
        private readonly ITokenList _tokenList;

        public AttributeController(IUserRepositoryExtendAdmin repository, ITokenList list)
        {
            _userRepository = repository ?? throw new ArgumentException(nameof(_userRepository));
            _tokenList = list ?? throw new ArgumentException(nameof(_tokenList));
        }

        /// <summary>
        /// 访客登陆，随机生产一个10位的账号，然后给一个DTO
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GuestLogin()
        {
            var guestToken = _tokenList.addUserAuthe(Math.Round(10.0).ToString(), new List<string>());
            return Ok($"Authorization:{guestToken}");
        }
    
        /// <summary>
        /// 用户登陆，验证账号成功后返回Token
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserSignReturnDto>> UserLogin(LoginDto login)
        {
            var userEntities = await _userRepository.GetUserAccount(login.Account);
            if (userEntities == null)
            {
                return NotFound("账号不存在");
            }

            if (userEntities.Credential != login.Credential)
            {
                return NotFound("账号密码错误");
            }
            
            var roleRights = await _userRepository.GetRoleRights(userEntities.RoleId);
            // 拿到用户的权限信息
            var list = roleRights.Select(x => x.Name).ToList();
            // 一起把权限和Token加到token_list，然后返回生成的Token
            var token = _tokenList.addUserAuthe(userEntities.Account, list);
            var userSignReturnDto =  new UserSignReturnDto
            {
                Token = token,
                UserId = await _userRepository.GetUserIdByAccount(login.Account)
            };
            return Ok(userSignReturnDto);
        }
        
        
        /// <summary>
        /// 过滤器：用户级别
        /// 用户登出，删除Token
        /// </summary>
        /// <returns></returns>
        [ServiceFilter(typeof(UserFilter))]
        [HttpGet]
        [Route("logout")]
        public ActionResult<bool> UserLogout()
        {
            var token = Request.Headers["Authorization"];
            // 从tokenlist删除token
            _tokenList.delUserToken(token);
            return Ok("登出成功！");
        }
    }
}