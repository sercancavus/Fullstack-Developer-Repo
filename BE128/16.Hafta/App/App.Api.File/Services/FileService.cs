using App.Models.DTO.File;
using App.Services;
using App.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.StaticFiles;

namespace App.Api.File.Services
{
    internal class FileService(IServiceProvider serviceProvider)
        : AppServiceBase(serviceProvider), IFileService
    {
        public async Task<Result> DeleteAsync(FileDeleteRequest fileRequest)
        {
            var validationResult = await ValidateModelAsync(fileRequest);
            if (!validationResult.IsSuccess)
            {
                return Result.Invalid(validationResult.ValidationErrors);
            }
            var filePath = Path.Combine(GetFileSaveFolder(), fileRequest.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                return Result.NotFound();
            }

            System.IO.File.Delete(filePath);

            return Result.NoContent();
        }

        public async Task<Result<FileDownloadResult>> DownloadAsync(FileDownloadRequest fileRequest)
        {
            var validationResult = await ValidateModelAsync(fileRequest);
            if (!validationResult.IsSuccess)
            {
                return Result.Invalid(validationResult.ValidationErrors);
            }
            var filePath = Path.Combine(GetFileSaveFolder(), fileRequest.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                return Result.NotFound();
            }

            string contentType = "application/octet-stream";

            if (new FileExtensionContentTypeProvider().TryGetContentType(fileRequest.FileName, out var ct))
            {
                contentType = ct;
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return Result<FileDownloadResult>.Success(new FileDownloadResult
            {
                ContentType = contentType,
                Content = fileBytes,
                Name = fileRequest.FileName
            });
        }

        public async Task<Result<FileUploadResult>> UploadAsync(FileUploadRequest fileRequest)
        {
            var validationResult = await ValidateModelAsync(fileRequest);
            if (!validationResult.IsSuccess)
            {
                return Result.Invalid(validationResult.ValidationErrors);
            }

            var filePath = Path.Combine(GetFileSaveFolder(), fileRequest.Name);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.CreateNew))
                {
                    await fileRequest.Stream.CopyToAsync(stream);
                }

                return Result<FileUploadResult>.Success(new FileUploadResult { FileName = fileRequest.Name });
            }
            catch (IOException)
            {
                return Result.Conflict("Dosya zaten mevcut.");
            }
        }

        private static string GetFileSaveFolder()
        {
            var fileSaveFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(fileSaveFolder))
            {
                Directory.CreateDirectory(fileSaveFolder);
            }

            return fileSaveFolder;
        }
    }
}
