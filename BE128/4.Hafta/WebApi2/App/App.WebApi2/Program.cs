var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/hi", () => 
{

    return "Siliconmade Academy";

});

int i = 0;

app.MapGet("/say" , () => {
    return i++;
});

// 3) 

List<string> list = new() {
    "Mahmut",
    "Cemil",
    "Suat" 
};

int counter = 0;

app.MapGet("/list", () => {
    // Birinci istekte ilk isimden ba�las�n
    // �kinci istekte ikinci isimden ba�las�n
    // ���nc� istekte ���nc� isimden ba�las�n
    // ���nc� isimden sonra ba�a d�ns�n
    // if olmadan yap�ls�n

    int index = counter % 3;
    counter++;
    return list[index];
});

app.Run();

