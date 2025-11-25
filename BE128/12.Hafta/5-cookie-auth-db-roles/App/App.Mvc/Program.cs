using App.Mvc.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth-cookie";
        options.Cookie.HttpOnly = true; // cookie'ye JS ile eriþilemesin
        options.Cookie.IsEssential = true; // Bu Cookie gerekli

        options.LoginPath = "/Auth/Login"; // Login olmamýþ bir kiþi nereye yönlendirilsin.(Login olmayý gerektiren sayfalarda login olunmamýþsa)


        options.AccessDeniedPath = "/Auth/AccessDenied"; // yetkisiz giriþlerde buraya yönlendir.

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
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (await dbContext.Database.EnsureCreatedAsync())
    {
        // Seed Data

        await DbSeeder.SeedAsync(dbContext);
    }

}

app.Run();
