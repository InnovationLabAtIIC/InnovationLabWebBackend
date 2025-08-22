using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace InnovationLab.Shared.Extensions;

public static class AppConfigurationExtensions
{
    public static IConfigurationBuilder AddDefaultConfiguration(this IConfigurationBuilder builder, IHostEnvironment env)
    {
        var basePath = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName!;

        builder
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder;
    }
}
