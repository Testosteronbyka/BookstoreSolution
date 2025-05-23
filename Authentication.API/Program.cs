using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Authentication.API.Data;
using Authentication.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
var connectionString = "Server=mysql.titanaxe.com;Port=3306;Database=srv306153;Uid=srv306153;Pwd=x4YqYfMt;CharSet=utf8mb4;";

// Konfiguracja DbContext
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29))));

// Konfiguracja Identity z łagodnymi wymaganiami
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;
    
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = false;
    options.Lockout.AllowedForNewUsers = false;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/test.html"));

// Test połączenia i tworzenie użytkowników
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("🔄 Testowanie połączenia z MySQL...");
        var canConnect = await context.Database.CanConnectAsync();
        
        if (canConnect)
        {
            logger.LogInformation("✅ Połączenie z MySQL OK");
            
            var userCount = await userManager.Users.CountAsync();
            logger.LogInformation($"👥 Użytkowników w bazie: {userCount}");
            
            if (userCount == 0)
            {
                logger.LogInformation("➕ Tworzę użytkowników testowych...");
                
                var testUsers = new[]
                {
                    new { Email = "admin@example.com", Password = "admin123" },
                    new { Email = "test@example.com", Password = "test123" },
                    new { Email = "user@example.com", Password = "user123" }
                };
                
                foreach (var testUser in testUsers)
                {
                    try
                    {
                        var user = new ApplicationUser
                        {
                            UserName = testUser.Email, 
                            Email = testUser.Email,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(user, testUser.Password);

                        
                        var result = await userManager.CreateAsync(user, testUser.Password);
                        
                        if (result.Succeeded)
                        {
                            logger.LogInformation($"✅ Utworzono {testUser.Email}");
                        }
                        else
                        {
                            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                            logger.LogError($"❌ Błąd tworzenia {testUser.Email}: {errors}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"💥 Wyjątek podczas tworzenia {testUser.Email}");
                    }
                }
                
                var finalCount = await userManager.Users.CountAsync();
                logger.LogInformation($"🎯 Końcowa liczba użytkowników: {finalCount}");
            }
        }
        else
        {
            logger.LogError("❌ Nie można połączyć z MySQL");
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Błąd inicjalizacji: {Message}", ex.Message);
    }
}

Console.WriteLine("🚀 Authentication.API działa na http://localhost:5002");
Console.WriteLine("🧪 Test: http://localhost:5002/api/auth/test");
Console.WriteLine("👤 Użytkownicy testowi:");
Console.WriteLine("   📧 admin@example.com / 🔑 admin123");
Console.WriteLine("   📧 test@example.com / 🔑 test123");
Console.WriteLine("   📧 user@example.com / 🔑 user123");

app.Run();
