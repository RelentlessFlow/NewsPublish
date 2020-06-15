using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.API.ApiAuthorization.Filter;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AdminServices;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;

namespace NewsPublish.API.ApiCommon.Controllers
{
    /// <summary>
    /// 文件上传管理接口
    /// </summary>
    [ServiceFilter(typeof(AutheFilter))]
    [ApiController]
    [Route("api_common/users/{userId}/file")]
    public class UserFileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUserRepositoryExtendAdmin _userRepository;

        public UserFileController(IWebHostEnvironment environment,IUserRepositoryExtendAdmin userRepository)
        {
            _environment = environment ?? throw new ArgumentException(nameof(environment));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(userRepository));
        }

        /// <summary>
        /// 通过用户ID获取用户文件夹下的全部文件
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>文件列表</returns>
        [HttpGet]
        public ActionResult<Dictionary<string,string>> GetUserFiles(Guid userId)
        {
            var fileFolderName = $"file_user/{userId}";
            var fileFolder = Path.Combine(_environment.WebRootPath, $"{fileFolderName}");
            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);
            
            DirectoryInfo folder = new DirectoryInfo(fileFolder);
            var fileList = new Dictionary<string,string>();
            
            foreach (FileInfo file in folder.GetFiles("*"))
            {
                fileList.Add(file.Name,$"https://{Request.Host.Value}/{fileFolderName}/{file.Name}");
            }

            return fileList;
        }
        
        /// <summary>
        /// 通过用户ID和文件名获取文件URL信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{fileName}")]
        public ActionResult<Dictionary<string,string>> GetUserFile(Guid userId,string fileName)
        {
            var fileFolderName = $"file_user/{userId}";
            var fileFolder = Path.Combine(_environment.WebRootPath, $"{fileFolderName}");
            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);
            
            DirectoryInfo folder = new DirectoryInfo(fileFolder);
            var fileList = new Dictionary<string,string>();
            
            foreach (FileInfo file in folder.GetFiles("*"))
            {
                if (fileName.Equals(file.Name))
                {
                    fileList.Add(file.Name,$"https://{Request.Host.Value}/{fileFolderName}/{file.Name}");
                }
                break;
            }

            return fileList;
        }
        
        /// <summary>
        /// 通过用户ID和文件名删除文件接口
        /// 过滤器：管理员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpDelete("{fileName}")]
        public ActionResult<bool> DeleteUserFile(Guid userId,string fileName)
        {
            var filePath = Path.Combine(_environment.WebRootPath, $"file_user/{userId}/{fileName}");
            if (System.IO.File.Exists(filePath))
            {
                if (System.IO.File.GetAttributes(filePath).ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)

                    System.IO.File.SetAttributes(filePath, FileAttributes.Normal);

                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 通过用户ID上传文件接口
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="formCollection">FormData文件传输对象</param>
        /// <returns></returns>
        [ServiceFilter(typeof(UserFilter))]
        [HttpPost]
        public async Task<IActionResult> PostFile(Guid userId,[FromForm] IFormCollection formCollection)
        {
            if (!await _userRepository.UserIsExists(userId))
            {
                return NotFound("用户不存在");
            }
            var file = formCollection.Files.GetFile("file");
            if (file == null)
            {
                return ValidationProblem("file为必填字段,上传文件为空");
            }
            var userEntity = await _userRepository.GetUser(userId);
            var webFile = MyTools.CreateWebFile(Request, formCollection, _environment, $"file_user/{userEntity.Id}");
            userEntity.Avatar = webFile.Url;
            await _userRepository.SaveAsync();
            Response.Headers["picUrl"] = webFile.Url;
            return Ok(new
            {
                Status = webFile.Result,
                PicURL = webFile.Url
            });
        }
    }
}