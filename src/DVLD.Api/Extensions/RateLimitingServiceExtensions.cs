using System.Threading.RateLimiting;
using DVLD.Api.Middleware;
using Microsoft.AspNetCore.RateLimiting;

namespace DVLD.Api.Extensions
{
    internal static class RateLimitingServiceExtensions
    {
        internal const string FixedWindowPolicy = "fixed-window";

        public static IServiceCollection AddApiRateLimiting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var permitLimit = configuration.GetValue("RateLimiting:PermitLimit", 100);
            var windowSeconds = configuration.GetValue("RateLimiting:WindowSeconds", 60);
            var queueLimit = configuration.GetValue("RateLimiting:QueueLimit", 0);

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy(FixedWindowPolicy, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = TimeSpan.FromSeconds(windowSeconds),
                            QueueLimit = queueLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }));

                options.OnRejected = async (context, cancellationToken) =>
                {
                    await RateLimitProblemDetails.WriteAsync(context.HttpContext, cancellationToken);
                };
            });

            return services;
        }
    }
}
