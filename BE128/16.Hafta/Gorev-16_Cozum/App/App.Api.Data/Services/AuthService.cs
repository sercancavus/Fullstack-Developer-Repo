using App.Data.Entities;
using App.Data.Infrastructure;
using App.Models.DTO.Auth;
using App.Models.DTO.Mail;
using App.Services;
using App.Services.Abstract;
using Ardalis.Result;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hasher = BCrypt.Net.BCrypt;

namespace App.Api.Data.Services
{
    public class AuthService(IServiceProvider serviceProvider) : AppServiceBase(serviceProvider), IAuthService
    {
        private IDataRepository Repo => GetRequiredService<IDataRepository>();

        public async Task<Result> ForgotPasswordAsync(AuthForgotPasswordRequest forgotPasswordRequest)
        {
            var validationResult = await ValidateModelAsync(forgotPasswordRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var user = await Repo.GetAll<UserEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == forgotPasswordRequest.Email);

            if (user is null)
            {
                return Result.NotFound();
            }

            user.ResetPasswordToken = Guid.NewGuid().ToString("n");

            var mailRequest = new MailSendRequest
            {
                To = [user.Email],
                Subject = "Şifre Sıfırlama",
                Body = $"Merhaba {user.FirstName}, <br> Şifrenizi sıfırlamak için <a href='https://localhost:5001/renew-password/{user.ResetPasswordToken}'>tıklayınız</a>.",
                IsHtml = true
            };

            var mailService = GetRequiredService<IMailService>();

            var mailResult = await mailService.SendAsync(mailRequest);

            if (!mailResult.IsSuccess)
            {
                return Result.Invalid(new ValidationError("Mail could not be sent"));
            }

            await Repo.UpdateAsync(user);
            return Result.Success();
        }

        public async Task<Result<AuthLoginResult>> LoginAsync(AuthLoginRequest loginRequest)
        {
            var validationResult = await ValidateModelAsync(loginRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var user = await Repo.GetAll<UserEntity>()
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

            if (user is null)
            {
                return Result<AuthLoginResult>.NotFound();
            }

            if (!user.Enabled)
            {
                return Result<AuthLoginResult>.Invalid(new ValidationError("User is disabled"));
            }

            if (!Hasher.Verify(loginRequest.Password, user.Password))
            {
                return Result<AuthLoginResult>.Unauthorized();
            }

            var loginResult = new AuthLoginResult
            {
                Token = GenerateToken(user)
            };

            return Result.Success(loginResult);
        }

        public async Task<Result<AuthRefreshTokenResult>> RefreshTokenAsync(AuthRefreshTokenRequest refreshTokenRequest)
        {
            var validationResult = await ValidateModelAsync(refreshTokenRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            JwtSecurityToken jwt;

            try
            {
                jwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenRequest.Token);
            }
            catch (Exception)
            {
                return Result.Invalid(new ValidationError("Invalid token"));
            }

            if (jwt.Issuer != Configuration["Jwt:Issuer"])
            {
                return Result.Invalid(new ValidationError("Invalid issuer"));
            }

            var userIdStr = jwt.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value;

            if (!int.TryParse(userIdStr, out var userId))
            {
                return Result.Invalid(new ValidationError("Invalid sub claim"));
            }

            var user = await Repo.GetAll<UserEntity>()
                .Include(x => x.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return Result<AuthRefreshTokenResult>.NotFound();
            }

            var refreshTokenResult = new AuthRefreshTokenResult
            {
                Token = GenerateToken(user)
            };

            return Result.Success(refreshTokenResult);
        }

        public async Task<Result> RegisterAsync(AuthRegisterRequest registerRequest)
        {
            var validationResult = await ValidateModelAsync(registerRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var userExists = await Repo.GetAll<UserEntity>()
                .AsNoTracking()
                .AnyAsync(x => x.Email == registerRequest.Email);

            if (userExists)
            {
                return Result.Invalid(new ValidationError("Email already exists"));
            }

            var user = new UserEntity
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Password = Hasher.HashPassword(registerRequest.Password),
                RoleId = 3,
                Enabled = true,
                HasSellerRequest = false
            };

            await Repo.AddAsync(user);

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(AuthResetPasswordRequest resetPasswordRequest)
        {
            var validationResult = await ValidateModelAsync(resetPasswordRequest);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var user = await Repo.GetAll<UserEntity>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x =>
                    x.ResetPasswordToken == resetPasswordRequest.Token
                    && x.Email == resetPasswordRequest.Email);

            if (user is null)
            {
                return Result.NotFound();
            }

            user.Password = Hasher.HashPassword(resetPasswordRequest.Password);
            user.ResetPasswordToken = null;

            await Repo.UpdateAsync(user);

            return Result.Success();
        }

        private string GenerateToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new(JwtClaimTypes.Subject, user.Id.ToString()),
                new(JwtClaimTypes.Name, user.FirstName),
                new(JwtClaimTypes.FamilyName, user.LastName),
                new(JwtClaimTypes.Email, user.Email),
                new(JwtClaimTypes.Role, user.Role.Name)
            };

            string secret = Configuration.GetRequiredSection("Jwt:Secret").Get<string>()!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            string issuer = Configuration.GetRequiredSection("Jwt:Issuer").Get<string>()!;

            var token = new JwtSecurityToken(
                issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
