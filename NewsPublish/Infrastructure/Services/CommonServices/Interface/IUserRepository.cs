using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsPublish.Database.Entities.RoleEntities;
using NewsPublish.Database.Entities.UserEntities;

namespace NewsPublish.Infrastructure.Services.CommonServices.Interface
{
    public interface IUserRepository
    {
        // 获取角色权限信息
        Task<IEnumerable<Right>> GetRoleRights(Guid roleId = new Guid());
        Task<IEnumerable<RightRole>> GetRightRoleEntitiesByRoleId(Guid roleId);
        void AddRightRole(RightRole rightRole);
        void DeleteRightRole(RightRole rightRole);

        // 下面的角色权限操作为多表操作
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        Task<IEnumerable<Role>> GetRoles();
        Task<Role> GetRole(Guid roleId);
        Task<Role> GetRole(string roleName);
        Task<bool> RoleIsExists(Guid roleId);
        Task<bool> RoleIsExists(string roleName);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<User> GetUser(Guid userId);
        
        Task<bool> UserIsExists(Guid userId);
        Task<bool> UserIsExists(string userName);
        void AddUserAuthe(Guid userId, UserAuthe userAuthe);
        void UpdateUserAuthe(UserAuthe userAuthe);
        void DeleteUserAuthe(UserAuthe userAuthe);
        Task<UserAuthe> GetUserAuthe(Guid userId, Guid userAutheId);
        Task<IEnumerable<UserAuthe>> GetUserAuthes(Guid userId);
        Task<bool> UserAutheIsExists(Guid userAutheId);
        Task<bool> UserAutheIsExists(string userAccount);
    
        /// <summary>
        /// 昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        Task<bool> UserNickNameIsExists(string nickName);
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<bool> UserAccountIsExists(string account);
        /// <summary>
        /// 根据用户账号查询ID
        /// </summary>
        /// <returns></returns>
        Task<Guid> GetUserIdByAccount(string account);


        Task<bool> SaveAsync();
        
        

    }
}