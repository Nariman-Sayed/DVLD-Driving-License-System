using DVLD.Api.Extensions;
using Scalar.AspNetCore;
using Serilog;

namespace DVLD.Api
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public sealed class Program
    {
        /// <summary>
        /// Configures and runs the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructurePersistenceServices(builder.Configuration);
            builder.Services.AddApiRateLimiting(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);

            var app = builder.Build();

            app.UseExceptionHandler();
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            Log.Information("Starting DVLD API");
            app.Run();
        }
    }
}
