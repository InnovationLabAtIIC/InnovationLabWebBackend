using CloudinaryDotNet;
using InnovationLab.Shared.Constants;
using InnovationLab.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnovationLab.Shared.Extensions;

public static class CloudinarySetupExtensions
{
    public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICloudinary, Cloudinary>(x =>
        {
            CloudinaryOptions cloudinaryOptions = new();
            configuration.GetSection(ConfigurationKeys.Cloudinary).Bind(cloudinaryOptions);

            var cloudinary = new Cloudinary(cloudinaryOptions.Url);
            cloudinary.Api.Secure = true;
            return cloudinary;
        });

        return services;
    }
}