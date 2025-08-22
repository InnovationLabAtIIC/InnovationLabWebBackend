using InnovationLab.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InnovationLab.Auth.DbContexts;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName!;

        // Load configuration from solution root
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath) // careful with path
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        var connString = config.GetConnectionString(ConfigurationKeys.PostgresConnection);
        optionsBuilder.UseNpgsql(connString);

        return new AuthDbContext(optionsBuilder.Options);
    }
}
