namespace DVLD.Api.Constants
{
    /// <summary>
    /// Named user-facing text constants used across the API.
    /// Localization is intentionally deferred; these constants are the single source of truth for now.
    /// </summary>
    public static class ApiErrorMessages
    {
        /// <summary>
        /// Generic unexpected error title.
        /// </summary>
        public const string UnexpectedError = "An unexpected error occurred";

        /// <summary>
        /// Problem type URI for validation errors.
        /// </summary>
        public const string ValidationProblemType = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

        /// <summary>
        /// Problem type URI for server errors.
        /// </summary>
        public const string ServerErrorProblemType = "https://tools.ietf.org/html/rfc9110#section-15.6.1";

        /// <summary>
        /// Message returned when login credentials are invalid.
        /// </summary>
        public const string InvalidCredentials = "Invalid username or password.";

        /// <summary>
        /// Message returned when the authenticated user id claim is missing.
        /// </summary>
        public const string MissingAuthenticatedUserId = "Authenticated user id claim is missing or invalid.";
    }
}
