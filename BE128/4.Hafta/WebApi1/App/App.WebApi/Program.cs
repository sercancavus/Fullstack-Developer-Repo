var builder = WebApplication.CreateBuilder(args);

// -------------- 1. KISIM --------------

// Add services to the container.

builder.Services.AddControllers(); // bu uygulamada Controller yap�s�n�n dahil edilmesi
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // endpoint yap�s�n�n dahil edilmesi
builder.Services.AddSwaggerGen(); // swagger'�n dahil edilmesi

var app = builder.Build(); // builder objesine dahil edilen yap�lar ile birlikte build edilmesi ve app isimli objeye aktar�lmas�

// app => web uygulamas�

// ---------------------------------------------------------------------------------------


// 2. KISIM

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // e�er uygulama geli�tirme ortam�ndaysa swagger kullan
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
