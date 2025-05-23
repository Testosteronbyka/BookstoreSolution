var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Użyj HTTP zamiast HTTPS żeby uniknąć problemów SSL
builder.Services.AddHttpClient("BookAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("AuthAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5002/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(1);
});

builder.Services.AddLogging();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// USUŃ app.UseHttpsRedirection(); żeby uniknąć problemów SSL
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();