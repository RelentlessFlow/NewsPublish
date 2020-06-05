using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NewsPublish.Authorization.ConfigurationModel;
using NewsPublish.Infrastructure.Services;
using NewsPublish.Infrastructure.Services.AuthorizeServices.DTO;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.Authorization.Filter
{
    /// <summary>
    /// 管理员权限过滤器
    /// </summary>
    public class AdminFilter : ActionFilterAttribute
    {
        private readonly IOptions<SystemFunctionOptionName> _nameOptions;
        private readonly ITokenList _list;
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;


        public AdminFilter(
            ITokenList tokenList, IUserRepository repository,
            IOptions<SystemFunctionOptionName> nameOptions,
            IConfiguration configuration)
        {
            _list = tokenList ?? throw new ArgumentException(nameof(tokenList));
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _nameOptions = nameOptions ?? throw new ArgumentException(nameof(nameOptions));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }
        
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 从请求体拿到Token
            var headerToken = context.HttpContext.Request.Headers["Authorization"];
            // 去tokenlist（一个依赖注入的list对象）查找token
            UserTokenWithRight userToken = _list.GetToken(headerToken);
            if (userToken == null)
            {
                var content = new UnauthorizedObjectResult("您的访问未经授权");
                context.Result = content;
            }
            else
            {
                // 看查找到的授权对象中是否包含AdminName的权限信息
                var contains = userToken.RightName.Contains(_nameOptions.Value.AdminName);
                if (!contains)
                {
                    var content = new UnauthorizedObjectResult("您的访问未经授权");
                    context.Result = content;
                }
            }
        }
    }
}