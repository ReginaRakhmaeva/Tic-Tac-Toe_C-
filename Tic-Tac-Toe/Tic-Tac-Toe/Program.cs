using Microsoft.EntityFrameworkCore;
using Tic_Tac_Toe.datasource.dbcontext;
using Tic_Tac_Toe.di;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Добавляем поддержку API контроллеров
builder.Services.AddControllers();

// Настройка подключения к PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Регистрируем зависимости через Configuration
Configuration.ConfigureDependencies(builder.Services);

var app = builder.Build();

// Настройте конвейер HTTP-запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // Подключаем API контроллеры

app.Run();
