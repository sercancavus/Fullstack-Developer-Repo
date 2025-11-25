using FluentValidation;

namespace App.Models.DTO.Auth
{
    public class AuthForgotPasswordRequest
    {
        public string Email { get; set; } = null!;
    }

    public class AuthForgotPasswordRequestValidator
        : AbstractValidator<AuthForgotPasswordRequest>
    {
        public AuthForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}