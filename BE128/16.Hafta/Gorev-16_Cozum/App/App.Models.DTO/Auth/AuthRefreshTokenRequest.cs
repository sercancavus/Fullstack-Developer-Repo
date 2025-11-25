using FluentValidation;

namespace App.Models.DTO.Auth
{
    public class AuthRefreshTokenRequest
    {
        public string Token { get; set; } = default!;
    }

    public class AuthRefreshTokenRequestValidator : AbstractValidator<AuthRefreshTokenRequest>
    {
        public AuthRefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}