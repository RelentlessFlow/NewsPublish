using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Database.Data;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Database.Entities.AuditEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AssessServices.DTO;
using NewsPublish.Infrastructure.Services.AssessServices.Interface;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;

namespace NewsPublish.Infrastructure.Services.AssessServices.Implementation
{
    public class ArticleReviewRepository : IArticleReviewRepository
    {
        private readonly RoutineDbContext _context;
        public ArticleReviewRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public void AddArticleReviewAudits(ArticleReviewAudit articleReviewAudit)
        {
            MyTools.ArgumentDispose(articleReviewAudit);
            articleReviewAudit.IsPass = false;
            articleReviewAudit.AuditStatus = false;
            articleReviewAudit.CreateTime = DateTime.Now;
            _context.ArticleReviewAudits.Add(articleReviewAudit);
        }

        public async Task<PagedList<ArticleReviewAuditsDto>> GetAllArticleReviewAudit(ArticleReviewAuditDtoParameters parameters)
        {
            MyTools.ArgumentDispose(parameters);

            var articlesIQ = _context.Articles as IQueryable<Article>;
            
            var queryExpression =
                from a in articlesIQ
                join u in _context.Users on a.UserId equals u.Id
                join c in _context.Categories on a.CategoryId equals c.Id
                join articleReview in _context.ArticleReviewAudits on a.Id equals articleReview.ArticleId 
                select new ArticleReviewAuditsDto
                {
                    CategoryName = c.Name,
                    Avatar = u.Avatar,
                    Introduction = u.Introduce,
                    CategoryId = c.Id,
                    CategoryRemark = c.Remark,
                    CoverPic = a.CoverPic,
                    ArticleCreateTime = a.CreateTime,
                    ArticleModifyTime = a.ModifyTime,
                    UserId = a.UserId,
                    UserName = u.NickName,
                    ArticleId = a.Id,
                    AticleTitle = a.Title,
                    State = a.States,
                    IsPass = articleReview.IsPass,
                    AuditStatus = articleReview.AuditStatus,
                    AuditCreateTime = articleReview.CreateTime,
                    AuditModifyTime = articleReview.CreateTime,
                    UserReviewAuditId = articleReview.Id,
                    ReviewRemark = articleReview.ReviewRemark,
                };
            
            // 精确查找：标签名字
            if (!string.IsNullOrWhiteSpace(parameters.TagName))
            {
                IEnumerable<Guid> tagIds = _context.Tags.Where(x => x.Name == parameters.TagName).Select(x => x.Id);
                var articleTagIds =
                    _context.ArticleTags
                        .Where(x => tagIds.Contains(x.TagId)).Select(a => a.ArticleId);
                queryExpression = queryExpression.Where(x => articleTagIds.Contains(x.ArticleId));
            }

            // 精确查找：标签ID
            if (parameters.TagId != Guid.Empty)
            {
                IEnumerable<Guid> tagIds = _context.Tags.Where(x => x.Id == parameters.TagId).Select(x => x.Id);
                var articleTagIds =
                    _context.ArticleTags
                        .Where(x => tagIds.Contains(x.TagId)).Select(a => a.ArticleId);
                queryExpression = queryExpression.Where(x => articleTagIds.Contains(x.ArticleId));
            }

            // 精确查找：分类名字
            if (!string.IsNullOrWhiteSpace(parameters.CategoryName))
                queryExpression = queryExpression
                    .Where(x => x.CategoryName == parameters.CategoryName);

            // 精确查找：分类ID
            if (parameters.CategoryId != Guid.Empty)
                queryExpression = queryExpression
                    .Where(x => x.CategoryId == parameters.CategoryId);

            // 精确查找：用户名字
            if (!string.IsNullOrWhiteSpace(parameters.UserName))
                queryExpression = queryExpression
                    .Where(x => x.UserName == parameters.UserName);

            // 精确查找：用户ID
            if (parameters.UserId != Guid.Empty)
                queryExpression = queryExpression
                    .Where(x => x.UserId == parameters.UserId);


            // 精确查找：ID
            if (parameters.NewsId != Guid.Empty)
                queryExpression = queryExpression
                    .Where(x => x.ArticleId == parameters.NewsId);

            // 模糊查询 标题和内容
            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                parameters.Q = parameters.Q.Trim();
                queryExpression = queryExpression
                    .Where(x => x.AticleTitle.Contains(parameters.Q));
            }
            
            // 精确查询 是否查询结果为未审核通过的文章
            if (parameters.isPass == false)
            {
                queryExpression = queryExpression
                    .Where(x => x.State == parameters.isPass);
            }
            
            
            // 模糊查询，查询文章名称
            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                parameters.Q = parameters.Q.Trim();
                queryExpression = queryExpression.Where(x => x.UserName.Contains(parameters.Q));
            }
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy == "ArticleCreateTime") queryExpression = queryExpression.OrderBy(x => x.ArticleCreateTime);
                if (parameters.OrderBy == "ArticleModifyTime") queryExpression = queryExpression.OrderBy(x => x.ArticleModifyTime);
                if (parameters.OrderBy == "AuditModifyTime") queryExpression = queryExpression.OrderBy(x => x.AuditModifyTime);
                if (parameters.OrderBy == "AuditCreateTime") queryExpression = queryExpression.OrderBy(x => x.AuditCreateTime);
                if (parameters.OrderBy == "IsPass") queryExpression = queryExpression.OrderBy(x => x.IsPass);
                if (parameters.OrderBy == "AuditStatus") queryExpression = queryExpression.OrderBy(x => x.AuditStatus);
                if (parameters.OrderBy == "UserName") queryExpression = queryExpression.OrderBy(x => x.UserName);
            }

            return await PagedList<ArticleReviewAuditsDto>
                .CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }


        public Task<ArticleReviewAuditsDto> GetArticleReviewAudit(Guid auditId)
        {
            MyTools.ArgumentDispose(auditId);

            var articlesIQ = _context.Articles as IQueryable<Article>;
            
            var queryExpression =
                from a in articlesIQ
                join u in _context.Users on a.UserId equals u.Id
                join c in _context.Categories on a.CategoryId equals c.Id
                join articleReview in _context.ArticleReviewAudits on a.Id equals articleReview.ArticleId 
                select new ArticleReviewAuditsDto
                {
                    CategoryName = c.Name,
                    Avatar = u.Avatar,
                    Introduction = u.Introduce,
                    CategoryId = c.Id,
                    CategoryRemark = c.Remark,
                    CoverPic = a.CoverPic,
                    ArticleCreateTime = a.CreateTime,
                    ArticleModifyTime = a.ModifyTime,
                    UserId = a.UserId,
                    UserName = u.NickName,
                    ArticleId = a.Id,
                    AticleTitle = a.Title,
                    State = a.States,
                    IsPass = articleReview.IsPass,
                    AuditStatus = articleReview.AuditStatus,
                    AuditCreateTime = articleReview.CreateTime,
                    AuditModifyTime = articleReview.CreateTime,
                    UserReviewAuditId = articleReview.Id,
                    ReviewRemark = articleReview.ReviewRemark
                };
            return queryExpression.FirstOrDefaultAsync();
        }

        public async Task<ArticleReviewAudit> GetArticleReviewAuditEntity(Guid auditId)
        {
            MyTools.ArgumentDispose(auditId);
            return await _context.ArticleReviewAudits.FirstOrDefaultAsync(x => x.Id == auditId);
        }

        public void DeleteArticleReviewAuditEntity(ArticleReviewAudit audit)
        {
            MyTools.ArgumentDispose(audit);
            _context.Remove(audit);
        }


        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}