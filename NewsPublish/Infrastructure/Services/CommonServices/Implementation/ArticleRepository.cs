using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Database.Data;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;
using NewsPublish.Infrastructure.Services.CommonServices.Interface;

namespace NewsPublish.Infrastructure.Services.CommonServices.Implementation
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly RoutineDbContext _context;

        public ArticleRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public void AddCategory(Category category)
        {
            MyTools.ArgumentDispose(category);
            _context.Categories.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            // _context.Entry(category).State = EntityState.Modified;
        }

        public void DeleteCategory(Category category)
        {
            _context.Remove(category);
        }

        public async Task<Category> GetCategory(Guid categoryId)
        {
            MyTools.ArgumentDispose(categoryId);
            return await _context.Categories.FirstOrDefaultAsync(
                x => x.Id == categoryId);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<bool> CategoryIsExists(Guid categoryId)
        {
            MyTools.ArgumentDispose(categoryId);
            return await _context.Categories.AnyAsync(x => x.Id == categoryId);
        }

        public async Task<bool> CategoryIsExists(string categoryName)
        {
            MyTools.ArgumentDispose(categoryName);
            return await _context.Categories.AnyAsync(x => x.Name == categoryName);
        }

        public async Task<PagedList<ArticleListDto>> GetArticles(ArticleDtoParameters parameters,
            bool findFailedArticle = false)
        {
            MyTools.ArgumentDispose(parameters);

            var articlesIQ = _context.Articles as IQueryable<Article>;

            if (findFailedArticle == false) articlesIQ = articlesIQ.Where(x => x.States);

            var queryExpression =
                from a in articlesIQ
                join u in _context.Users on a.UserId equals u.Id
                join c in _context.Categories on a.CategoryId equals c.Id
                select new ArticleListDto
                {
                    CategoryName = c.Name,
                    Avatar = u.Avatar,
                    Introduction = u.Introduce,
                    CategoryId = c.Id,
                    CategoryRemark = c.Remark,
                    CoverPic = a.CoverPic,
                    CreateTime = a.CreateTime,
                    ModifyTime = a.ModifyTime,
                    UserId = a.UserId,
                    UserName = u.NickName,
                    ArticleId = a.Id,
                    AticleTitle = a.Title,
                    State = a.States
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

            // 排序
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy == "CreateTime") queryExpression = queryExpression.OrderBy(x => x.CreateTime);
                if (parameters.OrderBy == "ModifyTime") queryExpression = queryExpression.OrderBy(x => x.ModifyTime);
                if (parameters.OrderBy == "Title") queryExpression = queryExpression.OrderBy(x => x.AticleTitle);
                if (parameters.OrderBy == "UserName") queryExpression = queryExpression.OrderBy(x => x.UserName);
                if (parameters.OrderBy == "CategoryName")
                    queryExpression = queryExpression.OrderBy(x => x.CategoryName);
            }

            return await PagedList<ArticleListDto>
                .CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }


        public async Task<List<ArticleDetailDto>> GetArticleDetail(Guid articleId)
        {
            MyTools.ArgumentDispose(articleId);

            var article = _context.Articles.Where(x => x.Id == articleId);

            var queryExpression =
                from a in article
                join u in _context.Users on a.UserId equals u.Id
                join c in _context.Categories on a.CategoryId equals c.Id
                select new ArticleDetailDto
                {
                    CategoryName = c.Name,
                    Avatar = u.Avatar,
                    Introduction = u.Introduce,
                    CategoryId = c.Id,
                    CategoryRemark = c.Remark,
                    CoverPic = a.CoverPic,
                    CreateTime = a.CreateTime,
                    ModifyTime = a.ModifyTime,
                    UserId = a.UserId,
                    UserName = u.NickName,
                    ArticleId = a.Id,
                    ArticleTitle = a.Title,
                    ArticleContent = a.Content
                };
            return await queryExpression.ToListAsync();
        }

        public void AddArticle(Guid categoryId, Guid userId, Article article)
        {
            MyTools.ArgumentDispose(categoryId);
            MyTools.ArgumentDispose(userId);
            MyTools.ArgumentDispose(article);
            article.UserId = userId;
            article.CategoryId = categoryId;
            article.CreateTime = DateTime.Now;
            article.ModifyTime = DateTime.Now;
            article.States = false;
            _context.Articles.Add(article);
        }

        public void UpdateArticle(Article article)
        {
            // _context.Entry(article).State = EntityState.Modified;
        }

        public void DeleteArticle(Article article)
        {
            MyTools.ArgumentDispose(article);
            _context.Articles.Remove(article);
        }

        public Task<Article> GetArticle(Guid articleId)
        {
            return _context.Articles.FirstOrDefaultAsync(x => x.Id == articleId);
        }

        public async Task<bool> ArticleIsExists(Guid articleId)
        {
            MyTools.ArgumentDispose(articleId);
            return await _context.Articles
                .AnyAsync(x => x.Id == articleId);
        }

        public async Task<bool> ArticleIsExists(string articleTitle)
        {
            MyTools.ArgumentDispose(articleTitle);
            return await _context.Articles
                .AnyAsync(x => x.Title == articleTitle);
        }

        public void AddTag(Tag tag)
        {
            MyTools.ArgumentDispose(tag);
            tag.CreateTime = DateTime.Now;
            _context.Tags.AddAsync(tag);
        }

        public void UpdateTag(Tag tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
        }

        public void DeleteTag(Tag tag)
        {
            _context.Remove(tag);
        }

        public async Task<Tag> GetTag(Guid tagId)
        {
            MyTools.ArgumentDispose(tagId);
            return await
                _context.Tags.FirstOrDefaultAsync(
                    x => x.Id == tagId);
        }

        public async Task<Tag> GetTag(string tagName)
        {
            MyTools.ArgumentDispose(tagName);
            return await
                _context.Tags.FirstOrDefaultAsync(
                    x => x.Name == tagName);
        }

        public async Task<IEnumerable<Tag>> GetArticleAllTags(Guid articleId)
        {
            MyTools.ArgumentDispose(articleId);

            // 现在有tagID了
            var articleTags = _context.ArticleTags.Where(x => x.ArticleId == articleId).Select(x => x.TagId);
            var tags = await _context.Tags.Where(x => articleTags.Contains(x.Id)).ToListAsync();
            return tags;
        }


        public async Task<bool> TagIsExists(Guid tagId)
        {
            MyTools.ArgumentDispose(tagId);
            return await _context.Tags
                .AnyAsync(x => x.Id == tagId);
        }

        public async Task<bool> TagIsExists(string tagName)
        {
            MyTools.ArgumentDispose(tagName);
            return await _context.Tags
                .AnyAsync(x => x.Name == tagName);
        }

        public async void AddArticleTag(ArticleTag articleTag)
        {
            MyTools.ArgumentDispose(articleTag);
            await _context.ArticleTags
                .AddAsync(articleTag);
        }

        public void UpdateTag(ArticleTag articleTag)
        {
            // _context.Entry<ArticleTag>().State = EntityState.Modified;
        }

        public void DeleteTag(ArticleTag articleTag)
        {
            MyTools.ArgumentDispose(articleTag);
            _context.ArticleTags.Remove(articleTag);
        }

        public async Task<bool> ArticleTagIsExists(Guid articleId, Guid tagId)
        {
            MyTools.ArgumentDispose(articleId);
            MyTools.ArgumentDispose(tagId);
            return await _context.ArticleTags
                .AnyAsync(x => x.ArticleId == articleId
                               && x.TagId == tagId);
        }

        public async Task<IEnumerable<ArticleTag>> GetArticleTags(Guid articleId)
        {
            MyTools.ArgumentDispose(articleId);
            return await _context.ArticleTags.Where(x => x.ArticleId == articleId).ToListAsync();
        }

        public async Task<Star> GetArticleStar(Guid articleId)
        {
            MyTools.ArgumentDispose(articleId);
            var star = await _context.Stars.FirstOrDefaultAsync(x => x.Type == StarType.文章 && x.StartId == articleId);
            return star;
        }

        public async void AddArticleStar(Guid articleId)
        {
            MyTools.ArgumentDispose(articleId);
            var star = await _context.Stars.FirstOrDefaultAsync(x => x.Type == StarType.文章 && x.StartId == articleId);
            if (star != null)
                star.Count += 1;
            else
                _context.Stars.Add(new Star
                {
                    StartId = articleId,
                    Type = StarType.文章,
                    Count = 1
                });
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<PagedList<Tag>> GetTags(TagDtoParameters parameters)
        {
            MyTools.ArgumentDispose(parameters);

            var queryExpression = _context.Tags as IQueryable<Tag>;

            // 精确查询:标签名称
            if (parameters.Name != null) queryExpression = queryExpression.Where(x => x.Name == parameters.Name);

            // 模糊查询：标签名称
            if (parameters.Q != null) queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.Q));

            // 处理排序
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
                if ("Name".Equals(parameters.OrderBy, StringComparison.CurrentCultureIgnoreCase))
                    queryExpression = queryExpression.OrderBy(x => x.Name);

            return await PagedList<Tag>
                .CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }
    }
}