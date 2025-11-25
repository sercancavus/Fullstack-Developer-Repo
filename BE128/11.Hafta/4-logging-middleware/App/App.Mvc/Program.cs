using App.Mvc.Middlewares;
using App.Mvc.Models.Config;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLogging(options =>
{
    options.ClearProviders(); // Default olarak gelen console logunu kaldýrýr.

    // Loglamanýn console'a yapýlacaðýný belirtir. (eski haline geri döner)
    //options.AddConsole();

    // loglarýn Event Viewer'a yapýlacaðýný belirtir.
    //options.AddEventLog();


    options.AddConsole();

});

builder.Services.Configure<UserConfigModel>(builder.Configuration.GetSection("User"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//app.Use(async (context, next) =>
//{
//    context.Response.Headers["X-Ornek"] = "BE128";
//    await next();
//});


//app.Use(async (context, next) =>
//{
//    // logger kullanabilmek için scope oluþturulur.
//    using var scope = app.Services.CreateScope();

//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

//    await next();

//    logger.LogInformation("{tarih} - {path} adresine {method} isteðinde bulunuldu.", DateTime.Now, context.Request.Path, context.Request.Method);

//    if (context.Response.StatusCode < 500 && context.Response.StatusCode >= 400)
//    {
//        logger.LogWarning("{tarih} - {path} adresine istekte bulunuldu. {statusCode} yanýtý alýndý", DateTime.Now, context.Request.Path, context.Response.StatusCode);
//    }

//});


app.UseMiddleware<RequestLoggerMiddleware>();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
