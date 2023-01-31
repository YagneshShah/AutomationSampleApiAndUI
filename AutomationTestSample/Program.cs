using AutomationTestSample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ITestDbContext, TestDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<TestDbContext>();
    try
    {
        logger.LogInformation("Checking for DB migration.");
        context.Database.EnsureCreated();
        logger.LogInformation("Migrations (if any) completed.");
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "Exception performing migration.");
    }
}

app.Run();
public partial class Program { } // Program class is created internally and to make it accessible in test project add this and reference it from tests project
