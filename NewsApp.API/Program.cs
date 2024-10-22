using Hangfire;
using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data;
using NewsApp.API.Data.Repository.Base;
using NewsApp.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string dbConnectionString = builder.Configuration.GetConnectionString("SqlServerConnection")!;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(dbConnectionString);
});

builder.Services.AddHangfire(x => x.UseSqlServerStorage(dbConnectionString));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<ExtractNewestArticlesService>();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var recurringJobManager = services.GetRequiredService<IRecurringJobManager>();
    var extractArticlesService = services.GetRequiredService<ExtractNewestArticlesService>();

    recurringJobManager.AddOrUpdate("Fetch and Save Articles",
        () => extractArticlesService.ParseAndSaveNewArticles(), "*/15 * * * *");
}

app.MapHangfireDashboard();

app.Run();
