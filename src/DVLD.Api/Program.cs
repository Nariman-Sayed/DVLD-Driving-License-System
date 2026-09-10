using DVLD.Api.Data.Interfaces;
using DVLD.Api.Data.Repositories;
using DVLD.Api.Middleware;
using DVLD.Api.Services.Implementations;
using DVLD.Api.Services.Interfaces;
using Scalar.AspNetCore;
using Serilog;

namespace DVLD.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/dvld-api-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<IPersonService, PersonService>();

            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            try
            {
                Log.Information("Starting DVLD API");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "DVLD API failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}