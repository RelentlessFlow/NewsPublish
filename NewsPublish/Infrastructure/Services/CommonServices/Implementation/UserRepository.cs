using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Database.Data;
using NewsPublish.Database.Entities.RoleEntities;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.Infrastructure.Services.CommonServices.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly RoutineDbContext _context;

        public UserRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<IEnumerable<Right>> GetRoleRights(Guid roleId= new Guid())
        {
            var q = _context.Rights as IQueryable<Right>;

            if (roleId != Guid.Empty)
            {
                // 先查找所有满足条件的Role，然后把Role的ID取出来
                IEnumerable<Role> roles = await _context.Roles.Where(x => x.Id == roleId).ToListAsync();
                IEnumerable<Guid> roleIds = roles.Select(x => x.Id);
                // 匹配所有中间所有满足刚才拿出来的Ids的 RightRole，然后取出来他的RightId，
                var roleRights =
                    _context.RightRoles
                        .Where(x => roleIds.Contains(x.RoleId)).Select(rr => rr.RightId);
                // 查找所有包含roleRights中满足Id的 q
                q = q.Where(x => roleRights.Contains(x.Id));
            }
            return await q.ToListAsync();
        }

        public async Task<IEnumerable<RightRole>> GetRightRoleEntitiesByRoleId(Guid roleId)
        {
            MyTools.ArgumentDispose(roleId);
            return await _context.RightRoles.Where(x => x.RoleId == roleId).ToListAsync();
        }

        public void AddRightRole(RightRole rightRole)
        {
            MyTools.ArgumentDispose(rightRole);
            _context.RightRoles.Add(rightRole);
        }

        public void DeleteRightRole(RightRole rightRole)
        {
            MyTools.ArgumentDispose(rightRole);
            _context.RightRoles.Remove(rightRole);
        }

        public void AddRole(Role role)
        {
            MyTools.ArgumentDispose(role);
            role.CreateTime = DateTime.Now;

            if (role.ModifyTime == new DateTime()) role.ModifyTime = DateTime.Now;
            _context.Roles.Add(role);
        }

        public void UpdateRole(Role role)
        {
            // _context.Entry(role).State = EntityState.Modified;
        }

        public void DeleteRole(Role role)
        {
            MyTools.ArgumentDispose(role);
            _context.Roles.Remove(role);
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRole(Guid roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
        }

        public async Task<Role> GetRole(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
        }


        public async Task<bool> RoleIsExists(Guid roleId)
        {
            MyTools.ArgumentDispose(roleId);
            return await _context.Roles.AnyAsync(x => x.Id == roleId);
        }

        public async Task<bool> RoleIsExists(string roleName)
        {
            MyTools.ArgumentDispose(roleName);
            return await _context.Roles.AnyAsync(x => x.Name == roleName);
        }

        public void AddUser(User user)
        {
            MyTools.ArgumentDispose(user.RoleId);
            MyTools.ArgumentDispose(user);
            user.States = true;
            _context.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            // _context.Entry(user).State = EntityState.Modified;
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User> GetUser(Guid userId)
        {
            MyTools.ArgumentDispose(userId);
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
        
        public async Task<PagedList<UserDto>> GetUsers(UserDtoParameters parameters)
        {
            MyTools.ArgumentDispose(parameters);
            var queryable = from user in _context.Set<User>()
                join role in _context.Set<Role>() on user.RoleId equals role.Id
                select new UserDto
                {
                    Id = user.Id,
                    States = user.States,
                    RoleId = role.Id,
                    Avatar = user.Avatar,
                    Introduce = user.Introduce,
                    Name = user.NickName,
                    RoleName = role.Name
                };
            
            // 按照用户ID查找用户
            if (parameters.UserId != Guid.Empty) queryable = queryable.Where(x => x.Id == parameters.UserId);

            // 按照权限查找用户
            if (parameters.RoleId != Guid.Empty) queryable = queryable.Where(x => x.RoleId == parameters.RoleId);
            // 按照权限名称查找用户
            if (parameters.RoleName != null) queryable = queryable.Where(x => x.RoleName == parameters.RoleName);


            // 模糊查询 可查昵称和介绍
            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                parameters.Q = parameters.Q.Trim();
                queryable = queryable
                    .Where(x => x.Name.Contains(parameters.Q));
            }

            // 处理排序
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
                if ("Name".Equals(parameters.OrderBy, StringComparison.CurrentCultureIgnoreCase))
                    queryable = queryable.OrderBy(x => x.Name);

            queryable = queryable.AsQueryable();

            return await PagedList<UserDto>
                .CreateAsync(queryable,
                    parameters.PageNumber,
                    parameters.PageSize);
        }


        public async Task<bool> UserIsExists(Guid userId)
        {
            MyTools.ArgumentDispose(userId);
            return await _context.Users
                .AnyAsync(x => x.Id == userId);
        }

        public async Task<bool> UserIsExists(string userName)
        {
            MyTools.ArgumentDispose(userName);
            return await _context.Users
                .AnyAsync(x => x.NickName == userName);
        }
        

        public void AddUserAuthe(Guid userId, UserAuthe userAuthe)
        {
            MyTools.ArgumentDispose(userId);
            MyTools.ArgumentDispose(userAuthe);

            userAuthe.UserId = userId;
            if (userAuthe.ModifyTime == new DateTime()) userAuthe.ModifyTime = DateTime.Now;
            userAuthe.RegisterTime = DateTime.Now;
            _context.UserAuthes.Add(userAuthe);
        }

        public void UpdateUserAuthe(UserAuthe userAuthe)
        {
            _context.Entry(userAuthe).State = EntityState.Modified;
        }

        public void DeleteUserAuthe(UserAuthe userAuthe)
        {
            _context.UserAuthes.Remove(userAuthe);
        }

        public Task<UserAuthe> GetUserAuthe(Guid userId, Guid userAutheId)
        {
            MyTools.ArgumentDispose(userId);
            MyTools.ArgumentDispose(userAutheId);
            return _context.UserAuthes.FirstOrDefaultAsync(
                x => x.UserId == userId
                     && x.Id == userAutheId);
        }


        public async Task<IEnumerable<UserAuthe>> GetUserAuthes(Guid userId)
        {
            MyTools.ArgumentDispose(userId);
            return await _context.UserAuthes
                .Where(x => x.UserId == userId).ToListAsync();
        }


        public async Task<bool> UserAutheIsExists(Guid userAutheId)
        {
            MyTools.ArgumentDispose(userAutheId);
            return await _context.UserAuthes
                .AnyAsync(x => x.Id == userAutheId);
        }

        public async Task<bool> UserAutheIsExists(string userAccount)
        {
            MyTools.ArgumentDispose(userAccount);
            return await _context.UserAuthes
                .AnyAsync(x => x.Account == userAccount);
        }

        public async Task<bool> UserNickNameIsExists(string nickName)
        {
            MyTools.ArgumentDispose(nickName);
            return await _context.Users
                .AnyAsync(x => x.NickName == nickName);
        }

        public async Task<bool> UserAccountIsExists(string account)
        {
            MyTools.ArgumentDispose(account);
            return await _context.UserAuthes
                .AnyAsync(x => x.Account == account);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<Guid> GetUserIdByAccount(string account)
        {
            var userAuthe = await _context.UserAuthes.FirstOrDefaultAsync(x => x.Account == account);
            return userAuthe.UserId;
        }
    }
}