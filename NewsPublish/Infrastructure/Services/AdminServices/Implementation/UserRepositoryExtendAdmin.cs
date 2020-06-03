using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Database.Data;
using NewsPublish.Database.Entities.RoleEntities;
using NewsPublish.Database.Entities.UserEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AdminServices.DTO;
using NewsPublish.Infrastructure.Services.AdminServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices;
using NewsPublish.Infrastructure.Services.CommonServices.Implementation;

namespace NewsPublish.Infrastructure.Services.AdminServices.Implementation
{
    public class UserRepositoryExtendAdmin : UserRepository,IUserRepositoryExtendAdmin
    {
        private readonly RoutineDbContext _context;
        public UserRepositoryExtendAdmin(RoutineDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task<UserDto> GetUserWithRole(Guid userId)
        {
            MyTools.ArgumentDispose(userId);
            return await _context.Users
                .Join(_context.Roles, u => u.RoleId, f => f.Id,
                    (u, r) => new UserDto
                    {
                        Id = u.Id,
                        Avatar = u.Avatar,
                        Introduce = u.Introduce,
                        Name = u.NickName,
                        RoleId = u.RoleId,
                        RoleName = r.Name,
                        States = u.States
                    }).FirstOrDefaultAsync(x => x.Id == userId);
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

        

        // 登陆返回信息
        public async Task<UserAccountDto> GetUserAccount(string account)
        {
            var q = _context.UserAuthes.Join(_context.Users, authe => authe.UserId, user => user.Id, (authe, user) =>
                new UserAccountDto
                {
                    Account = authe.Account,
                    AuthType = authe.AuthType.ToString(),
                    Credential = authe.Credential,
                    RoleId = user.RoleId
                });
            var dto = await q.FirstOrDefaultAsync(x => x.Account == account);
            return dto;
        }
    }
}