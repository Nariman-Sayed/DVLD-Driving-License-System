using DVLD.Api.Constants;
using DVLD.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Middleware
{
    /// <summary>
    /// Global exception handler that maps exceptions to RFC 7807 <see cref="ProblemDetails"/> responses.
    /// </summary>
    /// <param name="logger">The logger used to record exceptions.</param>
    internal sealed class GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        /// <inheritdoc />
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            int statusCode;
            string title;
            string type;
            string detail;
            LogLevel logLevel;

            switch (exception)
            {
                case ApplicationValidationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = exception.Message;
                    type = ApiErrorMessages.ValidationProblemType;
                    detail = exception.Message;
                    logLevel = LogLevel.Warning;
                    break;

                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    title = ApiErrorMessages.NotFoundTitle;
                    type = ApiErrorMessages.NotFoundProblemType;
                    detail = exception.Message;
                    logLevel = LogLevel.Warning;
                    break;

                case ConflictException:
                    statusCode = StatusCodes.Status409Conflict;
                    title = ApiErrorMessages.ConflictTitle;
                    type = ApiErrorMessages.ConflictProblemType;
                    detail = exception.Message;
                    logLevel = LogLevel.Warning;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    title = ApiErrorMessages.UnexpectedError;
                    type = ApiErrorMessages.ServerErrorProblemType;
                    detail = ApiErrorMessages.UnexpectedDetail;
                    logLevel = LogLevel.Error;
                    break;
            }

            logger.Log(
                logLevel,
                exception,
                "Exception {ExceptionType} while processing {Path}. TraceId: {TraceId}",
                exception.GetType().Name,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(
                problemDetails, (System.Text.Json.JsonSerializerOptions?)null, "application/problem+json", cancellationToken);

            return true;
        }
    }
}
