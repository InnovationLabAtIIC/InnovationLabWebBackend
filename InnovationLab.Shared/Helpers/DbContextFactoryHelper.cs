using InnovationLab.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InnovationLab.Shared.Helpers;

public static class DbContextFactoryHelper
{
    public static TContext CreateDbContext<TContext>(string[] args) where TContext : DbContext
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        var connString = config.GetConnectionString(ConfigurationKeys.PostgresConnection);
        optionsBuilder.UseNpgsql(connString);

        return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options)!;
    }
}