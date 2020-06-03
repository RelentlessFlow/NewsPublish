using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiSite.Models.Register;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiSite.Controllers
{
    /// <summary>
    /// 注册用户的接口
    /// </summary>
    [ApiController]
    [Route("api_site/register")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        
        public RegisterController(IUserRepository repository, IMapper mapper, IWebHostEnvironment environment)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _environment = environment ?? throw new ArgumentException(nameof(environment));
        }
        
        /// <summary>
        /// 注册用户接口
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns>注册之后的账号信息</returns>
        [HttpPost]
        public async Task<ActionResult<RegisterReturnDto>> RegisterUser(RegisterDto registerDto)
        {
            if (await _repository.UserNickNameIsExists(registerDto.NickName))
            {
                return ValidationProblem("用户名已存在！");
            }

            if (await _repository.UserAccountIsExists(registerDto.Account))
            {
                return ValidationProblem("账号已存在！");
            }
            
            var addToUser = _mapper.Map<User>(registerDto);
            // 设置默认的头像
            addToUser.Avatar = $"https://{Request.Host.Value}/default/avatar.jpeg";
            var addToAuthe = _mapper.Map<UserAuthe>(registerDto);
            _repository.AddUser(addToUser);
            _repository.AddUserAuthe(addToUser.Id, addToAuthe);
            await _repository.SaveAsync();
            RegisterReturnDto dto = new RegisterReturnDto
            {
                Id = addToUser.Id,
                Account = addToAuthe.Account,
                AccountType = addToAuthe.AuthType,
                NickName = addToUser.NickName,
                Avatar = addToUser.Avatar,
                Introduction = addToUser.Introduce,
                Password = addToAuthe.Credential,
                RegisterTime = addToAuthe.RegisterTime
            };
            return dto;
        }
        
        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpHead]
        [Route("{nickname}")]
        public async Task<ActionResult<bool>> IsExistNickName(string nickName)
        {
            return await _repository.UserNickNameIsExists(nickName);
        }
        
        /// <summary>
        /// 判断账户名是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpHead]
        [Route("{account}")]
        public async Task<ActionResult<bool>> IsExistAccount(string account)
        {
            return await _repository.UserNickNameIsExists(account);
        }
        
    }
}