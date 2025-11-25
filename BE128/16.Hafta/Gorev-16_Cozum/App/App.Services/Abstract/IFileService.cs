using App.Models.DTO.File;
using Ardalis.Result;

namespace App.Services.Abstract
{
    public interface IFileService
    {
        Task<Result> DeleteAsync(FileDeleteRequest fileRequest);
        Task<Result<FileDownloadResult>> DownloadAsync(FileDownloadRequest fileRequest);
        Task<Result<FileUploadResult>> UploadAsync(FileUploadRequest fileRequest);
    }
}
