using FluentValidation;

namespace App.Models.DTO.File
{
    public class FileDeleteRequest
    {
        public string FileName { get; set; } = string.Empty;
    }

    public class FileDeleteRequestValidator : AbstractValidator<FileDeleteRequest>
    {
        public FileDeleteRequestValidator()
        {
            RuleFor(x => x.FileName).NotEmpty()
                .MinimumLength(1)
                .Must(x => x.Contains('.'));
        }
    }
}