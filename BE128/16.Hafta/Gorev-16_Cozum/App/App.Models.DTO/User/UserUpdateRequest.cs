using FluentValidation;

namespace App.Models.DTO.User
{
    public class UserUpdateRequest
    {
        public int Id { get; set; }
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? ResetPasswordToken { get; set; } = null;
    }

    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).Must(x => x is null || x.Trim().Length > 0);
            RuleFor(x => x.LastName).Must(x => x is null || x.Trim().Length > 0);
            RuleFor(x => x.Password).Must(x => x is null || x.Trim().Length > 4);
            RuleFor(x => x.ResetPasswordToken).Must(x => x is null || x.Trim().Length > 0);

            RuleFor(x => x)
                .Must(x => x.FirstName is not null || x.LastName is not null || x.Password is not null || x.ResetPasswordToken is not null)
                .WithMessage("At least one field must be filled.");
        }
    }
}