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
using NewsPublish.Infrastructure.Services.CommonServices.DTO;
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