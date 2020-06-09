using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 创作者认证控制器
    /// </summary>
    public class CreatorAutheController
    {
        private readonly IUserRepository _userRepository;
        public CreatorAutheController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? 
                              throw new ArgumentException(nameof(IUserRepository));
        }
    }
}