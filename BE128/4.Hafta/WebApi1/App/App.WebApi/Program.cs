var builder = WebApplication.CreateBuilder(args);

// -------------- 1. KISIM --------------

// Add services to the container.

builder.Services.AddControllers(); // bu uygulamada Controller yapýsýnýn dahil edilmesi
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // endpoint yapýsýnýn dahil edilmesi
builder.Services.AddSwaggerGen(); // swagger'ýn dahil edilmesi

var app = builder.Build(); // builder objesine dahil edilen yapýlar ile birlikte build edilmesi ve app isimli objeye aktarýlmasý

// app => web uygulamasý

// ---------------------------------------------------------------------------------------


// 2. KISIM

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // eðer uygulama geliþtirme ortamýndaysa swagger kullan
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
