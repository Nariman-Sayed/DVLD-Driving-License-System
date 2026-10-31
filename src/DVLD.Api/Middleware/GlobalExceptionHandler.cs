using DVLD.Api.Constants;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Middleware
{
    /// <summary>
    /// Global exception handler that maps exceptions to RFC 7807 <see cref="ProblemDetails"/> responses.
    /// </summary>
    /// <param name="logger">The logger used to record exceptions.</param>
    /// <param name="environment">The hosting environment used to determine whether details are exposed.</param>
    internal sealed class GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment) : IExceptionHandler
    {
        /// <inheritdoc />
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            bool isValidationException = exception.GetType().FullName == "DVLD.Api.Exceptions.ApplicationValidationException";

            if (isValidationException)
            {
                logger.LogWarning(
                    exception,
                    "Validation exception while processing {Path}. TraceId: {TraceId}",
                    httpContext.Request.Path,
                    httpContext.TraceIdentifier);
            }
            else
            {
                logger.LogError(
                    exception,
                    "Unhandled exception while processing {Path}. TraceId: {TraceId}",
                    httpContext.Request.Path,
                    httpContext.TraceIdentifier);
            }

            int statusCode = isValidationException
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = isValidationException ? exception.Message : ApiErrorMessages.UnexpectedError,
                Type = isValidationException
                    ? ApiErrorMessages.ValidationProblemType
                    : ApiErrorMessages.ServerErrorProblemType,
                Instance = httpContext.Request.Path
            };

            if (isValidationException)
            {
                problemDetails.Detail = exception.Message;
            }
            else if (environment.IsDevelopment())
            {
                problemDetails.Detail = exception.ToString();
            }

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(
                problemDetails, (System.Text.Json.JsonSerializerOptions?)null, "application/problem+json", cancellationToken);

            return true;
        }
    }
}
