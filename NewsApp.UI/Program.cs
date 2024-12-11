using Blazored.LocalStorage;
using NewsApp.UI.Components;

using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;
using NewsApp.UI.Service;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CustomAuthenticationService>();
builder.Services.AddScoped<CommentService>();

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5296/api/")
});

/*
builder.Services.AddHttpClient("AuthorizedClient")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));
*/
builder.Services.AddHttpClient();

builder.Services.AddMudServices();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var app = builder.Build();


//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();