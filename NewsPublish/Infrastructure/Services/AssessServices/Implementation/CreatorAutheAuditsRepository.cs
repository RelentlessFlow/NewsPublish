using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Database.Data;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AssessServices.DTO;
using NewsPublish.Infrastructure.Services.AssessServices.Interface;

namespace NewsPublish.Infrastructure.Services.AssessServices.Implementation
{
    public class CreatorAutheAuditsRepository : ICreatorAutheAuditsRepository
    {
        private readonly RoutineDbContext _context;
        public CreatorAutheAuditsRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public void AddCreatorAutheAudits(CreatorAutheAudit creatorAutheAudit)
        {
            MyTools.ArgumentDispose(creatorAutheAudit);
            creatorAutheAudit.IsPass = false;
            creatorAutheAudit.AuditStatus = false;
            creatorAutheAudit.CreateTime = DateTime.Now;
            _context.CreatorAutheAudits.Add(creatorAutheAudit);
        }

        public async Task<PagedList<CreatorAutheAuditsDto>> GetAllCreatorAutheAudits(CreatorAutheAuditsDtoParameters parameters)
        {
            var queryExpression = _context.CreatorAutheAudits.Join(_context.Users, audit => audit.UserId, user => user.Id,
                (audit, user) => new CreatorAutheAuditsDto
                {
                    AuditStatus =  audit.AuditStatus,
                    Avatar = user.Avatar,
                    CreatorAutheAuditId = audit.Id,
                    Introduction = user.Introduce,
                    IsPass = audit.IsPass,
                    Remark = audit.Remark,
                    UserId = user.Id,
                    UserName = user.NickName,
                    CreateTime = audit.CreateTime
                });
            queryExpression = queryExpression.Where(x => x.UserId == parameters.userId);
            
            // 模糊查询，查询用户名称
            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                parameters.Q = parameters.Q.Trim();
                queryExpression = queryExpression.Where(x => x.UserName.Contains(parameters.Q));
            }
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy == "CreateTime") queryExpression = queryExpression.OrderBy(x => x);
                if (parameters.OrderBy == "Title") queryExpression = queryExpression.OrderBy(x => x.CreateTime);
                if (parameters.OrderBy == "IsPass") queryExpression = queryExpression.OrderBy(x => x.IsPass);
                if (parameters.OrderBy == "AuditStatus") queryExpression = queryExpression.OrderBy(x => x.AuditStatus);
                if (parameters.OrderBy == "UserName") queryExpression = queryExpression.OrderBy(x => x.UserName);
            }

            return await PagedList<CreatorAutheAuditsDto>
                .CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }


        public Task<CreatorAutheAuditsDto> GetCreatorAutheAudit(Guid auditId)
        {
            MyTools.ArgumentDispose(auditId);
            var creatorAutheAudit = _context.CreatorAutheAudits.Where(x => x.Id == auditId);
            var queryExpression = creatorAutheAudit.Join(_context.Users, audit => audit.UserId, user => user.Id,
                (audit, user) => new CreatorAutheAuditsDto
                {
                    AuditStatus =  audit.AuditStatus,
                    Avatar = user.Avatar,
                    CreatorAutheAuditId = audit.Id,
                    Introduction = user.Introduce,
                    IsPass = audit.IsPass,
                    Remark = audit.Remark,
                    UserId = user.Id,
                    UserName = user.NickName,
                    CreateTime = audit.CreateTime
                });
            return queryExpression.FirstOrDefaultAsync();
        }

        public async Task<CreatorAutheAudit> GetCreatorAutheAuditEntity(Guid auditId)
        {
            MyTools.ArgumentDispose(auditId);
            return await _context.CreatorAutheAudits.FirstOrDefaultAsync(x => x.Id == auditId);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}