using App.Models.DTO.Auth;
using App.Models.DTO.Mail;
using App.Services.Abstract;
using App.Services.Concrete;
using FluentValidation;

namespace App.Api.Data.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddAuthService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IValidator<AuthLoginRequest>, AuthLoginRequestValidator>();
            services.AddScoped<IValidator<AuthRefreshTokenRequest>, AuthRefreshTokenRequestValidator>();
            services.AddScoped<IValidator<AuthRegisterRequest>, AuthRegisterRequestValidator>();

            return services;
        }

        public static IServiceCollection AddEmailService(this IServiceCollection services)
        {
            services.AddScoped<IMailService, EmailService>();
            services.AddScoped<IValidator<MailSendRequest>, MailSendRequestValidator>();

            return services;
        }
    }
}
