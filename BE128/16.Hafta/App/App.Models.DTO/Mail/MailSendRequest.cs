using FluentValidation;

namespace App.Models.DTO.Mail
{
    public class MailSendRequest
    {
        public string[] To { get; set; } = null!;
        public string[]? Cc { get; set; }
        public string[]? Bcc { get; set; }
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsHtml { get; set; }
    }

    public class MailSendRequestValidator : AbstractValidator<MailSendRequest>
    {
        public MailSendRequestValidator()
        {
            RuleFor(x => x.To).NotEmpty().ForEach(x => x.EmailAddress());
            RuleFor(x => x.Cc).ForEach(x => x.EmailAddress());
            RuleFor(x => x.Bcc).ForEach(x => x.EmailAddress());
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}