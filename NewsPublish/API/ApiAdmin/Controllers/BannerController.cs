using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Authorization.Filter;
using NewsPublish.Database.Entities.WebEntities;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services;
using NewsPublish.Infrastructure.Services.CommonServices;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.API.ApiAdmin.Controllers
{
    /// <summary>
    /// Banner轮播图CURD
    /// 过滤器：管理员、授权用户
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ServiceFilter(typeof(AdminFilter))]
    [ApiController]
    [Route("api/banner")]
    public class BannerController : ControllerBase
    {
        private readonly IWebRepository _webRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public BannerController(IWebRepository webRepository,IMapper mapper,IWebHostEnvironment environment)
        {
            _webRepository = webRepository ?? 
                          throw new ArgumentNullException(nameof(webRepository));
            _mapper = mapper ?? 
                      throw new ArgumentNullException(nameof(mapper));
            _environment = environment ??
                           throw new ArgumentNullException(nameof(environment));
        }
        
        /// <summary>
        /// 获取的全部Banner信息
        /// </summary>
        /// <returns>全部的Banner信息</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Banner>>> GetBanners()
        {
            return Ok(await _webRepository.GetBanners());
        }
            
        /// <summary>
        /// 通过BannerID查询单个Banner详细信息
        /// </summary>
        /// <param name="bannerId"></param>
        /// <returns>一个Banner信息</returns>
        [HttpGet("{bannerId}" , Name = nameof(GetBanner))]
        public async Task<ActionResult<Banner>> GetBanner(Guid bannerId)
        {
            var banner = await _webRepository.GetBanner(bannerId);
            if (banner == null)
            {
                return NotFound();
            }

            return Ok(banner);
        }
        
        /// <summary>
        /// 创建Banner，传过来的值为FormData，包含Banner文件和Banner说明信息
        /// </summary>
        /// <param name="formCollection">包含文件、Banner名称、Banner备注的FormData文件</param>
        /// <returns>新的Banner的图片URL和操作结果</returns>
        [HttpPost]
        public async Task<ActionResult<Banner>> CreateBanner([FromForm] IFormCollection formCollection)
        {
            var file = formCollection.Files.GetFile("file");
            var url = formCollection["url"].ToString();
            var remark = formCollection["remark"].ToString();
            if (file==null||url.Equals("")||remark.Equals(""))
            {
                return ValidationProblem("file、url、remark为必填字段");
            }
            Guid bannerId = Guid.NewGuid();
            var webFile = MyTools.CreateWebFile(Request, formCollection, _environment, $"img_banner/{bannerId}");
            // 上传结果
            Boolean a = webFile.Result;
            if (!a)
            {
                return ValidationProblem("图片上传失败");
            }
            var picUrl = webFile.Url;
            var addToEntity = new Banner
            {
                Id = bannerId,
                Picture = picUrl,
                Remark = remark,
                Url = url
            };
            _webRepository.AddBanner(addToEntity);
            await _webRepository.SaveAsync();
            Response.Headers["picUrl"] = webFile.Url;
            return Ok(new
            {
                banner = addToEntity,
                PicURL = webFile.Url
            });
        }
    
        /// <summary>
        /// 通过Id删除Banner
        /// </summary>
        /// <param name="bannerId">Banner的ID</param>
        /// <returns>200状态吗</returns>
        [HttpDelete("{bannerId}")]
        public async Task<ActionResult<Banner>> DeleteBanner(Guid bannerId)
        {
            var banner = await _webRepository.GetBanner(bannerId);
            if (banner == null)
            {
                return NotFound();
            }
            if (banner.Picture != null)
            {
                var fileFolder = Path.Combine(_environment.WebRootPath, $"img_banner/{banner.Id}");
                MyTools.DeleteDirFile(fileFolder);
            }
            _webRepository.DeleteBanner(banner);
            await _webRepository.SaveAsync();
            return Ok();
        }
    }
}