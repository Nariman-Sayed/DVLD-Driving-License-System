using DVLD.Api.Data.Interfaces;
using DVLD.Api.Data.Repositories;
using DVLD.Api.Middleware;
using DVLD.Api.Services.Implementations;
using DVLD.Api.Services.Interfaces;

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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            services.AddScoped<IApplicationTypeService, ApplicationTypeService>();
            services.AddScoped<ITestTypeRepository, TestTypeRepository>();
            services.AddScoped<ITestTypeService, TestTypeService>();
            services.AddScoped<IBaseApplicationRepository, BaseApplicationRepository>();
            services.AddScoped<IBaseApplicationService, BaseApplicationService>();
            return services;
        }
    }
}
