namespace NewsPublish.Authorization.ConfigurationModel
{
    /// <summary>
    /// 对appsetting.json中权限名进行依赖注入的配置模型文件
    /// </summary>
    public class SystemFunctionOptionName
    {
        public string AdminName { get; set; }
        public string CreatorName { get; set; }
        public string AssessorName { get; set; }
        public string UserName { get; set; }
    }
}