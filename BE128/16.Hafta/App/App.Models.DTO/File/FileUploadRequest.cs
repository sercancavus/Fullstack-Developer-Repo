using FluentValidation;

namespace App.Models.DTO.File
{
    public class FileUploadRequest
    {
        public Stream Stream { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
    {
        public FileUploadRequestValidator()
        {
            RuleFor(x => x.Stream).NotNull();
            RuleFor(x => x.Name).NotEmpty()
                .MinimumLength(1)
                .Must(x => x.Contains('.'));
        }
    }
}