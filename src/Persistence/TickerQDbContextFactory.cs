using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TickerQ.EntityFrameworkCore.DbContextFactory;

namespace Persistence;

public class TickerQDbContextFactory : IDesignTimeDbContextFactory<TickerQDbContext>
{
    private readonly IConfiguration _configuration;

    public TickerQDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TickerQDbContext CreateDbContext(string[] args)
    {
        var connectionString = _configuration.GetConnectionString("TickerQ");

        var optionsBuilder = new DbContextOptionsBuilder<TickerQDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new TickerQDbContext(optionsBuilder.Options);
    }
}