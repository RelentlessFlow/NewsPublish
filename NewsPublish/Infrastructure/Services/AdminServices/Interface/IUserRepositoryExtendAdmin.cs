using System;
using System.Threading.Tasks;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AdminServices.DTO;
using NewsPublish.Infrastructure.Services.CommonServices;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.Infrastructure.Services.AdminServices.Interface
{
    /// <summary>
    /// EFCore 服务类
    /// </summary>
    public interface IUserRepositoryExtendAdmin : IUserRepository
    {
        /// <summary>
        /// 通过用户ID获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<UserDto> GetUserWithRole(Guid userId);
        

        // 获取用户验证信息
        Task<UserAccountDto> GetUserAccount(string account);
    }
}