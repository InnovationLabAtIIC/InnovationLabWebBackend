using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace InnovationLab.Shared.Extensions;

public static class AppConfigurationExtensions
{
    public static IConfigurationBuilder AddDefaultConfiguration(this IConfigurationBuilder builder, IHostEnvironment env)
    {
        builder
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder;
    }
}
