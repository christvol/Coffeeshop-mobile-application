using Common.Classes.DB;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Добавьте строку подключения
string? connectionString = builder.Configuration.GetConnectionString("TestingConnection");

// Добавьте контекст базы данных в контейнер служб
builder.Services.AddDbContext<CoffeeShopContext>(options =>
    options.UseSqlServer(connectionString));

// Добавьте контроллеры
builder.Services.AddControllers();

// Настройте Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Настройте конвейер обработки HTTP-запросов
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
