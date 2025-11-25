using App.Eticaret;
using App.Services.Abstract;
using App.Services.Concrete;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// add automapper for mapping view models to domain models (dtos)
builder.Services.AddAutoMapper(mapperConfiguration =>
{
    mapperConfiguration.AddProfile<MappingProfiles>();
});

// add http client for accessing data api
builder.Services.AddHttpClient("Api.Data", client =>
{
    var dataApiUrl = builder.Configuration["ExternalApis:DataApi"]
        ?? throw new InvalidOperationException("DataApi URL is missing");
    client.BaseAddress = new Uri(dataApiUrl);
});

// add http client for accessing file api
builder.Services.AddHttpClient("Api.File", client =>
{
    var fileApiUrl = builder.Configuration["ExternalApis:FileApi"]
        ?? throw new InvalidOperationException("FileApi URL is missing");
    client.BaseAddress = new Uri(fileApiUrl);
});

// add jwt bearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuerSigningKey = false,
        ValidateTokenReplay = false,
        SignatureValidator = (token, _) => new JsonWebToken(token),
    };

    // read jwt token from cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["auth-token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddScoped<IMailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthApiService>();
builder.Services.AddScoped<IFileService, FileApiService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();