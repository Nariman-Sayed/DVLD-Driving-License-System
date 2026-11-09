using System.Threading.RateLimiting;
using DVLD.Api.Middleware;
using DVLD.Api.Options;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DVLD.Api.Extensions
{
    internal static class RateLimitingServiceExtensions
    {
        internal const string FixedWindowPolicy = "fixed-window";
        internal const string AuthLoginPolicy = "auth-login";

        public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
        {
            services.AddOptions<RateLimitingOptions>()
                .BindConfiguration("RateLimiting")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                rateLimiterOptions.AddPolicy(FixedWindowPolicy, httpContext =>
                {
                    // Resolve at request time so test and environment configuration overrides
                    // are honoured instead of a snapshot taken during service registration.
                    var options = httpContext.RequestServices
                        .GetRequiredService<IOptions<RateLimitingOptions>>().Value;

                    return RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = options.PermitLimit,
                            Window = TimeSpan.FromSeconds(options.WindowSeconds),
                            QueueLimit = options.QueueLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });

                // A dedicated, stricter budget for the credential endpoint.
                rateLimiterOptions.AddPolicy(AuthLoginPolicy, httpContext =>
                {
                    var options = httpContext.RequestServices
                        .GetRequiredService<IOptions<RateLimitingOptions>>().Value;

                    return RateLimitPartition.GetFixedWindowLimiter(
                        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = options.LoginPermitLimit,
                            Window = TimeSpan.FromSeconds(options.LoginWindowSeconds),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });

                rateLimiterOptions.OnRejected = async (context, cancellationToken) =>
                {
                    await RateLimitProblemDetails.WriteAsync(context.HttpContext, cancellationToken);
                };
            });

            return services;
        }
    }
}
