using FluentValidation;

namespace App.Models.DTO.Auth
{
    public class AuthResetPasswordRequest
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordRepeat { get; set; } = null!;
    }

    public class AuthResetPasswordRequestValidator
        : AbstractValidator<AuthResetPasswordRequest>
    {
        public AuthResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(4);
            RuleFor(x => x.PasswordRepeat).Equal(x => x.Password);
        }
    }
}