using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using App.Eticaret.Infrastructure;
using App.Services.Abstract;
using App.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// HttpClients
builder.Services.AddHttpClient("FileApi", client =>
{
    var baseUrl = builder.Configuration["FileApi:BaseUrl"] ?? "https://localhost:5005";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddHttpClient("DataApi", client =>
{
    var baseUrl = builder.Configuration["DataApi:BaseUrl"] ?? "https://localhost:5004";
    client.BaseAddress = new Uri(baseUrl);
}).AddHttpMessageHandler<BearerTokenHandler>();

// Register business services for DI
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductCommentService, ProductCommentService>();

// JWT auth reading token from cookie
var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection["Issuer"] ?? "AppIssuer";
var audience = jwtSection["Audience"] ?? "AppAudience";
var secret = jwtSection["Secret"] ?? "SuperSecretKey_MinLen32_ChangeMe!SuperSecretKey";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["AuthToken"]; // JWT stored in cookie
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

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

app.Run();