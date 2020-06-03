using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;

namespace NewsPublish.Infrastructure.Services.CommonServices.Interface
{
    public interface IArticleRepository
    {
        // 分类和文章是一对多的关系，文章和用户也是一对多的关系
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Task<Category> GetCategory(Guid categoryId);
        Task<IEnumerable<Category>> GetCategories();
        Task<bool> CategoryIsExists(Guid categoryId);
        Task<bool> CategoryIsExists(string categoryName);
        
        
        Task<PagedList<ArticleListDto>> GetArticles(ArticleDtoParameters parameters, bool findFailedArticle = false);
        /// <summary>
        /// 获取文章详细信息
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<ArticleDetailDto>> GetArticleDetail(Guid articleId);
        
        void AddArticle(Guid categoryId,Guid userId,Article article);
        void UpdateArticle(Article article);
        void DeleteArticle(Article article);
        Task<Article> GetArticle(Guid articleId);
        /// <summary>
        /// 判断文章articleId是否存在
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<bool> ArticleIsExists(Guid articleId);
        /// <summary>
        /// 判断文章标题是否存在
        /// </summary>
        /// <param name="articleTitle"></param>
        /// <returns></returns>
        Task<bool> ArticleIsExists(string articleTitle);
        
        // 标签
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(Tag tag);
        Task<Tag> GetTag(Guid tagId);
        Task<Tag> GetTag(string tagName);
        Task<PagedList<Tag>> GetTags(TagDtoParameters parameters);

        
        // 获取文章标签
        Task<IEnumerable<Tag>> GetArticleAllTags(Guid articleId);

        Task<bool> TagIsExists(Guid tagId);
        Task<bool> TagIsExists(string tagName);
        // 文章标签 多对多
        void AddArticleTag(ArticleTag articleTag);
        void UpdateTag(ArticleTag articleTag);
        void DeleteTag(ArticleTag articleTag);
        Task<bool> ArticleTagIsExists(Guid articleId, Guid tagId);
        Task<IEnumerable<ArticleTag>> GetArticleTags(Guid articleId);
        Task<bool> SaveAsync();
    }
}