using App.Models.DTO.File;
using App.Services.Abstract;
using Ardalis.Result;
using System.Net;

namespace App.Services.Concrete
{
    public class FileApiService(IServiceProvider serviceProvider) : AppServiceBase(serviceProvider), IFileService
    {
        private HttpClient Client => GetRequiredService<IHttpClientFactory>()
            .CreateClient("Api.File");
        public async Task<Result> DeleteAsync(FileDeleteRequest fileRequest)
        {
            var validationResult = await ValidateModelAsync(fileRequest);
            if (!validationResult.IsSuccess)
            {
                return Result.Invalid(validationResult.ValidationErrors);
            }

            var response = await Client.DeleteAsync($"/file/?fileName={fileRequest.FileName}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            return Result.Success();
        }

        public async Task<Result<FileDownloadResult>> DownloadAsync(FileDownloadRequest fileRequest)
        {
            var validationResult = await ValidateModelAsync(fileRequest);
            if (!validationResult.IsSuccess)
            {
                return Result.Invalid(validationResult.ValidationErrors);
            }

            var response = await Client.GetAsync($"/file?fileName={fileRequest.FileName}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result.NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            var result = new FileDownloadResult
            {
                Name = response.Content.Headers.ContentDisposition?.FileNameStar
                    ?? response.Content.Headers.ContentDisposition?.FileName
                    ?? response.Content.Headers.ContentDisposition?.Name
                    ?? "file",
                ContentType = response.Content.Headers.ContentType?.MediaType
                    ?? "application/octet-stream",
                Content = fileBytes
            };

            return Result.Success(result);
        }

        public async Task<Result<FileUploadResult>> UploadAsync(FileUploadRequest fileRequest)
        {
            var validationResult = await ValidateModelAsync(fileRequest);
            if (!validationResult.IsSuccess)
            {
                return Result.Invalid(validationResult.ValidationErrors);
            }

            var content = new MultipartFormDataContent
            {
                { new StreamContent(fileRequest.Stream), "file", fileRequest.Name }
            };

            var response = await Client.PostAsync("/file", content);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Unavailable();
            }

            var fileId = await response.Content.ReadAsStringAsync();

            var result = new FileUploadResult
            {
                FileName = fileId
            };

            return Result.Success(result);
        }
    }
}
