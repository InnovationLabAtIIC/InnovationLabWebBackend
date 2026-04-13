using InnovationLab.Shared.Interfaces;
using InnovationLab.Shared.Repositories;
using InnovationLab.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InnovationLab.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}