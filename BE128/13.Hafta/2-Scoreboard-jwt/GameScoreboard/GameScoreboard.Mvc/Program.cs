//using GameScoreboard.Data.Entities;
using GameScoreboard.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<GameScoreboardDbContext>(options =>
//{
//    //
//    var user = new UserEntity();
//});

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string is not found.");

builder.Services.AddGameSbData(connectionString);


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.Name = "GameScoreboard.Cookie";
//        options.LoginPath = "/Auth/Login";
//    });


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // birisi token ile istekte bulunduðunda, token'ýn valide edilme kurallarý

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true, // Token üreten olsun
            ValidIssuer = "GameScoreboard", // kim olsun
            ValidateAudience = true, // token kullanan olsun
            ValidAudience = "MVC", // kim olsun
            ValidateLifetime = true, // zaman, nbf ve exp arasýnda mý?
            ValidateIssuerSigningKey = true, // jwt'nin son kýsmýnýn (signature) kontrol edilip edilmeyeceðini ayarlýyoruz.

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };

        options.Events = new JwtBearerEvents
        {
            // her http isteði geldiðinde
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Cookies["access_token"];

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },

            // Login.Path için;

            OnChallenge = async context =>
            {
                // zorlama davranýþýný engelle
                context.HandleResponse();

                // Þu adrese yönlendir.
                context.Response.Redirect("/Auth/Login");

                await Task.CompletedTask;
            }


        };

        options.MapInboundClaims = false;
        // kendi oluþturduðumuz claim key isimleri olursa, bunlarý soap mimarisine göre map'lemesin 

        // email ->  ............../Email


    });





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


// --------------------------------
app.UseAuthentication();
// --------------------------------


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DbContext>();

    await context.EnsureCreatedAndSeedAsync();
}

app.Run();
