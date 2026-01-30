using Microsoft.EntityFrameworkCore;
using Tic_Tac_Toe.datasource.dbcontext;
using Tic_Tac_Toe.di;
using Tic_Tac_Toe.web.middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Добавляем поддержку API контроллеров с глобальным фильтром авторизации
builder.Services.AddControllers(options =>
{
    options.Filters.Add<UserAuthenticatorAttribute>();
});

// Настройка подключения к PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Регистрируем зависимости через Configuration
Configuration.ConfigureDependencies(builder.Services);

var app = builder.Build();

// Автоматическое создание базы данных при старте
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        DatabaseInitializer.Initialize(context, logger);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Критическая ошибка при инициализации базы данных: {Message}", ex.Message);
        // Не останавливаем приложение, но логируем ошибку
    }
}

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
