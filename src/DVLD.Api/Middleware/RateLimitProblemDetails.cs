using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Middleware
{
    internal static class RateLimitProblemDetails
    {
        public static async Task WriteAsync(HttpContext context, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status429TooManyRequests,
                Title = "Too many requests",
                Type = "https://tools.ietf.org/html/rfc6585#section-4",
                Instance = context.Request.Path,
                Detail = "Request rate limit exceeded. Please retry later."
            };
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(
                problemDetails,
                (System.Text.Json.JsonSerializerOptions?)null,
                "application/problem+json",
                cancellationToken);
        }
    }
}
