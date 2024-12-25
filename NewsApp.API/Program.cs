using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data;
using NewsApp.API.Data.Repository.Base;
using NewsApp.API.Services;
using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsApp.API.Atributes;
using NewsApp.API.Data.Entities;
using NewsApp.API.mapper;
using NewsApp.API.Services.Initializers;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models.Dto.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, serverVersion, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(
        new MySqlStorage(
            connectionString,
            new MySqlStorageOptions
            { 
                QueuePollInterval = TimeSpan.FromSeconds(10),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 25000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Hangfire",
            }
        )
    ));

builder.Services.AddHangfireServer();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
                options.DefaultScheme =
                    options.DefaultSignInScheme =
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        ),
        RoleClaimType = ClaimTypes.Role
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Default", policy =>
        policy.RequireRole(UserRoles.Default, UserRoles.Premium, UserRoles.Admin));
        
    options.AddPolicy("Premium", policy =>
        policy.RequireRole(UserRoles.Premium, UserRoles.Admin));
        
    options.AddPolicy("Admin", policy =>
        policy.RequireRole(UserRoles.Admin));
    
});

builder.Services.AddRazorPages();

builder.Services.AddServicesWithAttribute(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

builder.Services.AddAutoMapper(typeof(MappingProfile));
var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
config.AssertConfigurationIsValid();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<RoleInitializerService>();

builder.Services.AddHttpClient<LiqPayService>();
builder.Services.AddScoped<LiqPayService>();

builder.Services.AddScoped<IValueResolver<User, UserDto, string>, UserRoleResolver>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    //.WithOrigins("https://localhost:44351))
    .SetIsOriginAllowed(origin => true));

app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var recurringJobManager = services.GetRequiredService<IRecurringJobManager>();
    var extractArticlesService = services.GetRequiredService<ExtractNewestArticlesService>();

    recurringJobManager.AddOrUpdate("Fetch and Save Articles",
        () => extractArticlesService.ParseAndSaveNewArticles(), "*/15 * * * *");
}

app.MapHangfireDashboard();

app.UseHangfireDashboard();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleInitializer = scope.ServiceProvider.GetRequiredService<RoleInitializerService>();
    var categoryInitializer = scope.ServiceProvider.GetRequiredService<CategoryInitializerService>();
    var userInitializer = scope.ServiceProvider.GetRequiredService<UserInitializerService>();
    
    await roleInitializer.InitializeAsync();
    await categoryInitializer.InitializeAsync();
    await userInitializer.InitializeAsync();
}

app.Run();
