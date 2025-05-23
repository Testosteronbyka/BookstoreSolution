using Microsoft.EntityFrameworkCore;
using BookStore.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Default"),
        new MySqlServerVersion(new Version(8, 0, 29))));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Dodaj logging
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// Test połączenia z bazą danych przy starcie
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<BookDbContext>();
        var canConnect = await context.Database.CanConnectAsync();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        if (canConnect)
        {
            logger.LogInformation(" Połączenie z bazą danych MySQL udane");
            var bookCount = await context.Books.CountAsync();
            logger.LogInformation($" Liczba książek w bazie: {bookCount}");
        }
        else
        {
            logger.LogError(" Nie można połączyć się z bazą danych MySQL");
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, " Błąd podczas testowania połączenia z bazą danych");
    }
}

app.Run();