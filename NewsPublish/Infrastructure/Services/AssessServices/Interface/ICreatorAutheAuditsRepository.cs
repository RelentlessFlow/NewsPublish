using System;
using System.Collections;
using System.Threading.Tasks;
using NewsPublish.Database.Data;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AssessServices.DTO;

namespace NewsPublish.Infrastructure.Services.AssessServices.Interface
{
    public interface ICreatorAutheAuditsRepository
    {
        void AddCreatorAutheAudits(CreatorAutheAudit creatorAutheAudit);
        Task<PagedList<CreatorAutheAuditsDto>> GetAllCreatorAutheAudits(CreatorAutheAuditsDtoParameters parameters);
        Task<CreatorAutheAuditsDto> GetCreatorAutheAudit(Guid auditId);
        Task<CreatorAutheAudit> GetCreatorAutheAuditEntity(Guid auditId);
        Task<bool> SaveAsync(); 
    }
}