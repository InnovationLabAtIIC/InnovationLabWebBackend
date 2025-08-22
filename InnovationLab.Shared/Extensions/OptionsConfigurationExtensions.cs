using InnovationLab.Shared.Constants;
using InnovationLab.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnovationLab.Shared.Extensions;

public static class OptionsConfigurationExtensions
{
    public static IServiceCollection AddOptionsConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(ConfigurationKeys.Jwt));
        services.Configure<SmtpOptions>(configuration.GetSection(ConfigurationKeys.Smtp));

        return services;
    }
}