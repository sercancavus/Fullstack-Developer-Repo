using Microsoft.EntityFrameworkCore;
using App.Data;
using Microsoft.AspNetCore.Identity;
using App.Data.Entities;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext and repository DI
builder.Services.AddDbContext<App.Data.AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

builder.Services.AddScoped(typeof(App.Data.Repositories.DataRepository<>));

// Add Identity and cookie auth
builder.Services.AddIdentity<App.Data.Entities.User, Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<App.Data.AppDbContext>()
    .AddDefaultTokenProviders();

// Relax password rules to allow simple passwords for testing
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

var app = builder.Build();

// Seed database, roles and users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Ensure database created
        context.Database.EnsureCreated();

        // Ensure AspNetUsers table has the new columns (for existing DBs)
        DbConnection conn = context.Database.GetDbConnection();
        conn.Open();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "PRAGMA table_info('AspNetUsers')";
            var hasIsActive = false;
            var hasIsSellerRequested = false;
            var hasDisplayName = false;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var colName = reader.GetString(1);
                    if (colName == "IsActive") hasIsActive = true;
                    if (colName == "IsSellerRequested") hasIsSellerRequested = true;
                    if (colName == "DisplayName") hasDisplayName = true;
                }
            }

            if (!hasDisplayName)
            {
                using var alter = conn.CreateCommand();
                alter.CommandText = "ALTER TABLE AspNetUsers ADD COLUMN DisplayName TEXT";
                alter.ExecuteNonQuery();
            }
            if (!hasIsSellerRequested)
            {
                using var alter = conn.CreateCommand();
                alter.CommandText = "ALTER TABLE AspNetUsers ADD COLUMN IsSellerRequested INTEGER NOT NULL DEFAULT 0";
                alter.ExecuteNonQuery();
            }
            if (!hasIsActive)
            {
                using var alter = conn.CreateCommand();
                alter.CommandText = "ALTER TABLE AspNetUsers ADD COLUMN IsActive INTEGER NOT NULL DEFAULT 1";
                alter.ExecuteNonQuery();
            }
        }
        conn.Close();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        string[] roles = new[] { "Buyer", "Seller", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed users for each role if not exists with simpler passwords
        var adminEmail = "admin@example.com";
        var sellerEmail = "seller@example.com";
        var buyerEmail = "buyer@example.com";
        var adminPassword = "admin123";
        var sellerPassword = "seller123";
        var buyerPassword = "buyer123";

        async Task EnsureUser(string email, string role, string displayName, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User { UserName = email, Email = email, DisplayName = displayName };
                var res = await userManager.CreateAsync(user, password);
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(user, role))
                    await userManager.AddToRoleAsync(user, role);
            }
        }

        await EnsureUser(adminEmail, "Admin", "System Admin", adminPassword);
        await EnsureUser(sellerEmail, "Seller", "Sample Seller", sellerPassword);
        await EnsureUser(buyerEmail, "Buyer", "Sample Buyer", buyerPassword);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
