using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NewsPublish.API.ApiAuthorization.ConfigurationModel;
using NewsPublish.Infrastructure.Services.AuthorizeServices.DTO;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiAuthorization.Filter
{
    public class CreatorFilter : ActionFilterAttribute
    {
        private readonly IOptions<SystemFunctionOptionName> _nameOptions;
        private readonly ITokenList _list;
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;


        public CreatorFilter(
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
            var headerToken = context.HttpContext.Request.Headers["Authorization"];
            UserTokenWithRight userToken = _list.GetToken(headerToken);
            if (userToken == null)
            {
                var content = new UnauthorizedObjectResult("您的访问未经授权");
                context.Result = content;
            }
            else
            {
                var contains = userToken.RightName.Contains(_nameOptions.Value.CreatorName);
                if (!contains)
                {
                    var content = new UnauthorizedObjectResult("您的访问未经授权");
                    context.Result = content;
                }
            }
            
        }
    }
}