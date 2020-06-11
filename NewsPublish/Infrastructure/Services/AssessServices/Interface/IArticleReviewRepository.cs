using System;
using System.Threading.Tasks;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AssessServices.DTO;

namespace NewsPublish.Infrastructure.Services.AssessServices.Interface
{
    public interface IArticleReviewRepository
    {
        void AddArticleReviewAudits(ArticleReviewAudit articleReviewAudit);
        Task<PagedList<ArticleReviewAuditsDto>> GetAllArticleReviewAudit(ArticleReviewAuditDtoParameters parameters);
        Task<ArticleReviewAuditsDto> GetArticleReviewAudit(Guid auditId);
        Task<ArticleReviewAudit> GetArticleReviewAuditEntity(Guid auditId);
        void DeleteArticleReviewAuditEntity(ArticleReviewAudit audit);
        Task<bool> SaveAsync();
    }
}