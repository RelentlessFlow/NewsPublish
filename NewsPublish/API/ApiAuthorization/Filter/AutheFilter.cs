using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Interface;

namespace NewsPublish.API.ApiAuthorization.Filter
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class AutheFilter : ActionFilterAttribute    
    {
        private readonly ITokenList _list;
        
        public AutheFilter(ITokenList tokenList)
        {
            _list = tokenList ?? throw new ArgumentException(nameof(tokenList));
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 拿到Header中的Token，然后直接判断token_list中有没有，没有就返回401
            var headerToken = context.HttpContext.Request.Headers["Authorization"];
            if (!_list.isExistAuth(headerToken))
            {
                var content = new UnauthorizedObjectResult("您的访问未经授权");
                context.Result = content;
            }
        }
    }
}