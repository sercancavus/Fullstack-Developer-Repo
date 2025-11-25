var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    // session'în set ettiði cookie'nin ayarlarý yapýlýr.

    // IsEssential -> bu cookie'nin olmazsa olmaz olduð anlamýna gelir.
    options.Cookie.IsEssential = true;

    // cookie'nin adý
    options.Cookie.Name = "Siliconmade.Session";

    // HttpOnly = true => cookie'nin JS ile eriþilemez olmasýný saðlar. Güvenliði artýrmak için kullanýr.
    options.Cookie.HttpOnly = true;


    options.IdleTimeout = TimeSpan.FromMinutes(20);

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


// --------------------------

// session kullan
app.UseSession();

// --------------------------


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
