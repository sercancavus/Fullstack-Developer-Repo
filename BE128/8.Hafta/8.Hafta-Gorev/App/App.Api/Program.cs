using App.Domain;
using Microsoft.EntityFrameworkCore;
using Bogus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=StudentDb;Trusted_Connection=True;";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();

// Ensure database is created and seed data if empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Students.Any())
    {
        var faker = new Faker<Student>()
            .RuleFor(s => s.FirstName, f => f.Name.FirstName())
            .RuleFor(s => s.LastName, f => f.Name.LastName())
            .RuleFor(s => s.StudentNumber, f => f.UniqueIndex + 1000)
            .RuleFor(s => s.BirthDate, f => f.Date.Past(25, DateTime.Now.AddYears(-18)))
            .RuleFor(s => s.Class, f => f.Random.String2(3, "ABCDEF"));
        var students = faker.Generate(20);
        db.Students.AddRange(students);
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
