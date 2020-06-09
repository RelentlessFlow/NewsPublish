using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    [ApiController]
    [Route("api_test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("test");
        }
    }
}