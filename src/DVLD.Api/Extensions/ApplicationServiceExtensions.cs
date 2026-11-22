using DVLD.Api.Data.Interfaces;
using DVLD.Api.Data.Repositories;
using DVLD.Api.Middleware;
using DVLD.Api.Options;
using DVLD.Api.Services.Implementations;
using DVLD.Api.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DVLD.Api.Extensions
{
    internal static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            services.AddScoped<IApplicationTypeService, ApplicationTypeService>();
            services.AddScoped<ITestTypeRepository, TestTypeRepository>();
            services.AddScoped<ITestTypeService, TestTypeService>();
            services.AddScoped<IBaseApplicationRepository, BaseApplicationRepository>();
            services.AddScoped<IBaseApplicationService, BaseApplicationService>();
            services.AddScoped<IPersonImageService, PersonImageService>();

            // Image storage is resolved from configuration when the endpoint is used,
            // so a missing secret never prevents the rest of the API from starting.
            services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                return new StorageOptions
                {
                    PersonImagesPath =
                        configuration[$"{StorageOptions.SectionName}:PersonImagesPath"] ?? string.Empty
                };
            });

            return services;
        }
    }
}
