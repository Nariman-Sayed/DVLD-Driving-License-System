using System.Net;

namespace DVLD.Desktop.Services
{
    /// <summary>
    /// Represents a failed call to the DVLD API with a message that is safe to show to the user.
    /// </summary>
    public sealed class ApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="message">The user-facing error message.</param>
        /// <param name="statusCode">The HTTP status code, when the request reached the API.</param>
        /// <param name="innerException">The underlying failure, when there is one.</param>
        public ApiException(string message, HttpStatusCode? statusCode = null, Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Gets the HTTP status code returned by the API, or <see langword="null"/> for network failures.
        /// </summary>
        public HttpStatusCode? StatusCode { get; }

        /// <summary>
        /// Gets a value indicating whether the API rejected the request as unauthorized.
        /// </summary>
        public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;
    }
}
