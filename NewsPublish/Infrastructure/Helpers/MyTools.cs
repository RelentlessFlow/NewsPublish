using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace NewsPublish.Infrastructure.Helpers
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public class MyTools
    {
        // 处理参数空指针异常
        public static void ArgumentDispose(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            else if (obj.GetType() == typeof(string))
            {
                if (obj.ToString() == "")
                {
                    Console.WriteLine("字符串为空");
                    throw new ArgumentNullException();
                }
            }
            else if (obj.GetType() == typeof(Guid))
            {
                if ((Guid) obj == Guid.Empty)
                {
                    Console.WriteLine("Guid");
                    throw new ArgumentNullException();
                }
            }
        }
        /// <summary>
        /// 删除文件夹下所有文件
        /// </summary>
        /// <param name="path1"></param>
        /// <returns></returns>
        public bool DeleteFile(string path1)
        {
            try
            {
                if (File.Exists(path1))
                {
                    File.Delete(path1);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 删除文件夹及其下面所有文件
        /// </summary>
        /// <param name="file">文件夹路径</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void DeleteDirFile(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                DirectoryInfo fileInfo = new DirectoryInfo(file);
                //去除文件夹的只读属性
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDirFile(f);
                        }
                    }

                    //删除空文件夹
                    Directory.Delete(file);
                }

            }
            catch (FileNotFoundException ex) // 异常处理
            {
                throw ex;
            }
        }
    
        /// <summary>
        /// 上传web文件到本地wwwroot
        /// </summary>
        /// <param name="request">请求体</param>
        /// <param name="formCollection">传过来的formdata</param>
        /// <param name="env">IWebHostEnvironment对象</param>
        /// <param name="folderName">文件夹名</param>
        /// <returns></returns>
        public static ReturnWebFileStates CreateWebFile(HttpRequest request,IFormCollection formCollection, IWebHostEnvironment env,
            string folderName)
        {
            bool result = false;
            string Url = "";
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            foreach (IFormFile file in fileCollection)
            {
                string fileFolderName = $"{folderName}";
                var fileFolder = Path.Combine(env.WebRootPath, $"{fileFolderName}");
                if (!Directory.Exists(fileFolder))
                    Directory.CreateDirectory(fileFolder);
                
                StreamReader reader = new StreamReader(file.OpenReadStream());
                String name = file.FileName;
                String filename = fileFolder + "/" + name;
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (FileStream fs = File.Create(filename))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }
                
                result = true;
                Url = $"https://{request.Host.Value}/{fileFolderName}/{name}" ;
            }

            return new ReturnWebFileStates
            {
                Url = Url,
                Result = result
            };
        }
        
        public class ReturnWebFileStates
        {
            public bool Result { get; set; }
            public string Url { get; set; }
        }
    }
}