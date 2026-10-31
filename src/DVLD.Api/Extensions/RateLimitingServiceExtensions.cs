using System.Threading.RateLimiting;
using DVLD.Api.Middleware;
using DVLD.Api.Options;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace DVLD.Api.Extensions
{
    internal static class RateLimitingServiceExtensions
    {
        internal const string FixedWindowPolicy = "fixed-window";

        public static IServiceCollection AddApiRateLimiting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions<RateLimitingOptions>()
                .BindConfiguration("RateLimiting")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var options = new RateLimitingOptions();
            configuration.GetSection("RateLimiting").Bind(options);

            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                rateLimiterOptions.AddPolicy(FixedWindowPolicy, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = options.PermitLimit,
                            Window = TimeSpan.FromSeconds(options.WindowSeconds),
                            QueueLimit = options.QueueLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }));

                rateLimiterOptions.OnRejected = async (context, cancellationToken) =>
                {
                    await RateLimitProblemDetails.WriteAsync(context.HttpContext, cancellationToken);
                };
            });

            return services;
        }
    }
}
