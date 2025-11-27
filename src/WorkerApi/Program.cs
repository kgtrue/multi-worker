using System.Net;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Scalar.AspNetCore;
using TickerQ.Caching.StackExchangeRedis.DependencyInjection;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.EntityFrameworkCore.DbContextFactory;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("TickerQ");

builder.Services.RegisterPersistence(builder.Configuration);

builder.Services.AddTickerQ(options =>
{
    options.AddDashboard(dashboardOptions =>
    {
        // Custom base path
        dashboardOptions.SetBasePath("/admin/jobs");
    });
    options.AddOperationalStore(efOptions =>
    {
        // Use built-in TickerQDbContext with connection string
        efOptions.UseTickerQDbContext<TickerQDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connectionString,
                cfg =>
                {
                    cfg.MigrationsAssembly("Persistence");
                    cfg.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), ["40P01"]);
                });
        });

        // Optional: Configure pool size
        efOptions.SetDbContextPoolSize(34);
    });


    options.AddStackExchangeRedis(redisOptions =>
    {
        redisOptions.Configuration = "redis:6379";
        redisOptions.InstanceName = "redis-tickerq:";
        redisOptions.NodeHeartbeatInterval = TimeSpan.FromMinutes(1);
    });

    options.ConfigureScheduler(scheduler => { scheduler.NodeIdentifier = Dns.GetHostName(); });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("docs2");

    app.UseReDoc(c =>
    {
        c.RoutePrefix = "docs";
        c.SpecUrl = "/openapi/v1.json";
    });
}

app.UseTickerQ();
app.UseHttpsRedirection();
app.Run();