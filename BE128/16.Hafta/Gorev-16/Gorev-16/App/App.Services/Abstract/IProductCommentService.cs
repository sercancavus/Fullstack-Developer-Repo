using Ardalis.Result;
using App.Models.DTO;

namespace App.Services.Abstract;

public interface IProductCommentService
{
    Task<Result<List<ProductCommentDto>>> ListPendingCommentsAsync(string jwt);
    Task<Result> ApproveCommentAsync(string jwt, int commentId);
}
