using BookStore.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// Test endpointu
app.MapGet("/test", () => "BookStore.API działa poprawnie!");

Console.WriteLine(" BookStore.API działa na http://localhost:5001");
Console.WriteLine(" Swagger: http://localhost:5001/swagger");
Console.WriteLine(" Test: http://localhost:5001/test");
Console.WriteLine(" API: http://localhost:5001/api/books");

app.Run();