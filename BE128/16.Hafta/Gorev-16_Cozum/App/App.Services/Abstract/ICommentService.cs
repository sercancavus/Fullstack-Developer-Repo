using App.Models.DTO.Comment;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface ICommentService
    {
        Task<Result<CommentGetResult>> GetAsync(int commentId);
        Task<Result<List<CommentGetResult>>> GetByProductIdAsync(int productId);
        Task<PagedResult<List<CommentGetResult>>> GetPagedAsync(CommentGetPagedRequest getRequest);
        Task<Result<CommentCreateResult>> CreateAsync(CommentCreateRequest commentRequest);
        Task<Result<CommentUpdateResult>> UpdateAsync(CommentUpdateRequest commentRequest);
        Task<Result<CommentApproveResult>> ApproveAsync(int commentId);
        Task<Result<CommentDeleteResult>> DeleteAsync(int commentId);
    }
}
