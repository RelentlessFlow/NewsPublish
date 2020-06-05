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
    public class CommentRepository : ICommentRepository
    {
        private readonly RoutineDbContext _context;

        public CommentRepository(RoutineDbContext context)
        {
            _context = context ?? throw new ArgumentException();
        }
        
        public async void AddComments(Guid articleId, Comment comment)
        {
            MyTools.ArgumentDispose(articleId);
            MyTools.ArgumentDispose(comment);
            comment.ArticleId = articleId;
            await _context.AddAsync(articleId);
        }

        public void UpdateComment(Comment comment)
        {
            // _context.Entry(comment).State = EntityState.Modified;
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        

        public async Task<Comment> GetComment(Guid commentId)
        {
            MyTools.ArgumentDispose(commentId);
            return await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
        }

        
        public async Task<PagedList<CommentListDto>> GetComments(Guid articleId, CommentDtoParameters parameters)
        {
            MyTools.ArgumentDispose(articleId);
            MyTools.ArgumentDispose(parameters);
            var queryExpression =
                from c in _context.Comments
                join a in _context.Articles
                    on c.ArticleId equals a.Id
                join u in _context.Users
                    on a.UserId equals u.Id
                select new CommentListDto
                {
                    Id = c.Id,
                    ArticleId = a.Id,
                    ArticleTitle = a.Title,
                    CreateTime = c.CreateTime,
                    UserId = u.Id,
                    UserName = u.NickName,
                    Content = c.Content,
                    AvatarUrl = u.Avatar
                };

            // 文章ID查找
            if (articleId != Guid.Empty) queryExpression = queryExpression.Where(x => x.ArticleId == articleId);
            
            //处理排序，默认按照评论时间先后排序
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy == "CreateTime")
                    queryExpression = queryExpression.OrderBy(x => x.CreateTime);
                else if (parameters.OrderBy == "ArticleId") queryExpression = queryExpression.OrderBy(x => x.ArticleId);
            }

            return await PagedList<CommentListDto>
                .CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }

        
        public async Task<bool> CommentIsExists(Guid commentId)
        {
            MyTools.ArgumentDispose(commentId);
            return await _context.Comments.AnyAsync(x => x.Id == commentId);
        }

        public async void AddReply(Reply reply)
        {
            MyTools.ArgumentDispose(reply);
            await _context.Replies.AddAsync(reply);
        }

        public void UpdateReply(Reply reply)
        {
            // _context.Entry(reply).State = EntityState.Modified;
        }

        public void DeleteReply(Reply reply)
        {
            MyTools.ArgumentDispose(reply);
            _context.Replies.Remove(reply);
        }

        public async Task<IEnumerable<Reply>> GetRepliesByComment(Guid commentId)
        {
            MyTools.ArgumentDispose(commentId);
            return await _context.Replies.Where(x => x.CommentId == commentId).ToListAsync();
        }
        

        public async Task<Reply> GetReply(Guid replyId)
        {
            MyTools.ArgumentDispose(replyId);
            return await _context.Replies.FirstAsync(x => x.Id == replyId);
        }

        public Task<Star> GetCommentStar(Guid commentId)
        {
            MyTools.ArgumentDispose(commentId);
            return _context.Stars.FirstOrDefaultAsync(x => x.Type == StarType.评论 && x.StartId == commentId);
        }

        public async void AddCommentStar(Guid commentId)
        {
            MyTools.ArgumentDispose(commentId);
            var star = await _context.Stars.FirstOrDefaultAsync(x => x.Type == StarType.评论 && x.StartId == commentId);
            if (star != null)
            {
                star.Count += 1;
            }
            else
            {
                _context.Stars.Add(new Star
                {
                    Type = StarType.评论,
                    Count = 1,
                    StartId = commentId
                });
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}