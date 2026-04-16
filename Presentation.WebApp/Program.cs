using Application;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add references to application and infrastructure projects
builder.Services.AddApplication(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(x => x.LowercaseUrls = true);

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// This Initializer will create the database if it doesn't exist,
// and seed it with default data (default roles and default admin accounts).
await InfrastructureInitializer.InitializeAsync(app.Services);

app.Run();
