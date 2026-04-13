using Application;

var builder = WebApplication.CreateBuilder(args);

// Add references to application and infrastructure projects
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

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

// Just wait for InfrastructureInitializer.InitializeAsync() to be built

app.Run();
