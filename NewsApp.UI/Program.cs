using Blazored.LocalStorage;
using MudBlazor.Services;
using NewsApp.Shared.Constants;
using NewsApp.UI.Service;
using NewsApp.UI.Components;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/newsapp-.txt", 
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Urls.ApiUrl) });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<TokenService>();


builder.Services.AddScoped<CustomAuthenticationService>();


builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<CommentService>();

builder.Services.AddScoped<AricleService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication();

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}