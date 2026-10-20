using DVLD.Api.Data;
using DVLD.Api.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Extensions
{
    internal static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddInfrastructurePersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DVLDConnection")
                ?? throw new InvalidOperationException("Connection string 'DVLDConnection' not found.");

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
