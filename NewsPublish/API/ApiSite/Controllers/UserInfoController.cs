using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NewsPublish.API.ApiAuthorization.ConfigurationModel;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.API.ApiSite.Models.User;
using NewsPublish.Database.Entities.RoleEntities;
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
    [ServiceFilter(typeof(AutheFilter))]
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
        /// 获取单个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<UserInfoDto>> GetUserInfo(Guid id)
        {
            if (! await _repository.UserIsExists(id))
            {
                return NotFound();
            }
            var user = await _repository.GetUser(id);
            var userInfoDto = _mapper.Map<UserInfoDto>(user);
            var roleRights = await _repository.GetRoleRights(user.RoleId);
            var names = roleRights.Select(x => x.Name);
            userInfoDto.isCreator = false;
            if (names.Contains(_nameOptions.Value.CreatorName))
            {
                userInfoDto.isCreator = true;
            }

            return userInfoDto;
        }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("nickName")]
        public async Task<IActionResult> ChangUserNickName(Guid id ,string nickName)
        {
            var user = await _repository.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            // if (await _repository.UserNickNameIsExists(nickName))
            // {
            //     return ValidationProblem("昵称已经存在");
            // }
            
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
        [HttpPut]
        [Route("avatarUrl")]
        public async Task<IActionResult> ChangUserAvatar(Guid id, string avatarUrl)
        {
            var user = await _repository.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            
            
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
        [HttpPut]
        [Route("introduce")]
        public async Task<IActionResult> ChangUserIntroduce(Guid id, string introduce)
        {
            var user = await _repository.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            
            user.Introduce = introduce;
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}