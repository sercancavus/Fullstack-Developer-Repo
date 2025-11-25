using App.Models.DTO.File;
using App.Services.Abstract;
using FluentValidation;

namespace App.Api.File.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddFileService(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<IValidator<FileDeleteRequest>, FileDeleteRequestValidator>();
            services.AddScoped<IValidator<FileDownloadRequest>, FileDownloadRequestValidator>();
            services.AddScoped<IValidator<FileUploadRequest>, FileUploadRequestValidator>();

            return services;
        }
    }
}
