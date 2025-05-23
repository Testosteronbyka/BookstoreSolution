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

builder.Services.AddLogging();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// USUŃ app.UseHttpsRedirection(); 
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// Test połączenia przy starcie
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<BookDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation(" Testowanie połączenia z bazą danych...");
        var canConnect = await context.Database.CanConnectAsync();
        
        if (canConnect)
        {
            logger.LogInformation(" Połączenie z MySQL OK");
            var bookCount = await context.Books.CountAsync();
            logger.LogInformation($" Książek w bazie: {bookCount}");
            
            // Jeśli brak książek, dodaj przykładowe
            if (bookCount == 0)
            {
                logger.LogInformation(" Dodaję przykładowe książki...");
                await context.Books.AddRangeAsync(
                    new BookStore.API.Models.Book 
                    {
                        Title = "Wiedźmin: Ostatnie życzenie",
                        Author = "Andrzej Sapkowski",
                        Price = 39.99m,
                        Stock = 10,
                        Description = "Pierwszy tom kultowej sagi o wiedźminie",
                        ISBN = "9788375780635",
                        CreatedDate = DateTime.Now
                    },
                    new BookStore.API.Models.Book 
                    {
                        Title = "Hobbit",
                        Author = "J.R.R. Tolkien",
                        Price = 49.99m,
                        Stock = 5,
                        Description = "Klasyczna powieść fantasy",
                        ISBN = "9788324159819",
                        CreatedDate = DateTime.Now
                    },
                    new BookStore.API.Models.Book 
                    {
                        Title = "1984",
                        Author = "George Orwell",
                        Price = 29.99m,
                        Stock = 8,
                        Description = "Dystopijny świat przyszłości",
                        ISBN = "9788382022287",
                        CreatedDate = DateTime.Now
                    }
                );
                await context.SaveChangesAsync();
                logger.LogInformation(" Dodano przykładowe książki");
            }
        }
        else
        {
            logger.LogError(" Nie można połączyć z bazą MySQL");
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, " Błąd bazy danych: {Message}", ex.Message);
    }
}

Console.WriteLine(" BookStore.API działa na http://localhost:5001");
app.Run();
