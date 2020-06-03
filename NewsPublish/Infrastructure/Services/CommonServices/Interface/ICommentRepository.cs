using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsPublish.Database.Entities.ArticleEntities;
using NewsPublish.Infrastructure.DtoParameters;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.CommonServices.DTO;

namespace NewsPublish.Infrastructure.Services.CommonServices.Interface
{
    public interface ICommentRepository
    {
        // 评论
        void AddComments(Guid articleId, Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(Comment comment);
        Task<Comment> GetComment(Guid commentId);
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PagedList<CommentListDto>> GetComments(Guid articleId, CommentDtoParameters parameters);

        Task<bool> CommentIsExists(Guid commentId);
        void AddReply(Reply reply);
        void UpdateReply(Reply reply);
        void DeleteReply(Reply reply);
        Task<IEnumerable<Reply>> GetRepliesByComment(Guid commentId);
        Task<Reply> GetReply(Guid replyId);
        Task<bool> SaveAsync();
    }
}