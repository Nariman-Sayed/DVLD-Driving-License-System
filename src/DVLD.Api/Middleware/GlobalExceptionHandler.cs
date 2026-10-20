using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Middleware
{
    internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(
                exception,
                "Unhandled exception while processing {Path}. TraceId: {TraceId}",
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Instance = httpContext.Request.Path
            };
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(
                problemDetails, (System.Text.Json.JsonSerializerOptions?)null, "application/problem+json", cancellationToken);

            return true;
        }
    }
}
