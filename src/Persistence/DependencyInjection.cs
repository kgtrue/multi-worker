using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerQ.EntityFrameworkCore.DbContextFactory;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection RegisterPersistence(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("TickerQ")
            ?? throw new InvalidOperationException("Connection string"
                                                   + "'DefaultConnection' not found.");
        
        serviceCollection.AddDbContext<TickerQDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        return serviceCollection;
    }
}