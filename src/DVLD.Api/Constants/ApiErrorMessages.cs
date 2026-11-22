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

        /// <summary>
        /// Problem type URI for not found responses.
        /// </summary>
        public const string NotFoundProblemType = "https://tools.ietf.org/html/rfc9110#section-15.5.5";

        /// <summary>
        /// Problem type URI for conflict responses.
        /// </summary>
        public const string ConflictProblemType = "https://tools.ietf.org/html/rfc9110#section-15.5.10";

        /// <summary>
        /// Title for not found responses.
        /// </summary>
        public const string NotFoundTitle = "Not found";

        /// <summary>
        /// Title for conflict responses.
        /// </summary>
        public const string ConflictTitle = "Conflict";

        /// <summary>
        /// Safe detail used for unexpected server errors.
        /// </summary>
        public const string UnexpectedDetail = "The request could not be completed.";

        /// <summary>
        /// Message used when a person with the same national number already exists.
        /// </summary>
        public const string DuplicateNationalNo = "A person with the same national number already exists.";

        /// <summary>
        /// Message used when a person is not found.
        /// </summary>
        public const string PersonNotFound = "The requested person does not exist.";

        /// <summary>
        /// Message used when a country is not found.
        /// </summary>
        public const string CountryNotFound = "The requested country does not exist.";

        /// <summary>
        /// Message used when a person cannot be deleted because other records reference it.
        /// </summary>
        public const string PersonDeleteBlocked = "The person cannot be deleted because other records reference it.";

        /// <summary>
        /// Message used when a person image is missing or is not a saved file name.
        /// </summary>
        public const string PersonImageNotFound = "The requested person image does not exist.";

        /// <summary>
        /// Message used when an uploaded image fails size or magic byte validation.
        /// </summary>
        public const string InvalidImageFile = "The image must be a JPEG or PNG file no larger than 2 MB.";

        /// <summary>
        /// Message used when the image storage directory is not configured.
        /// </summary>
        public const string ImageStorageNotConfigured = "Person image storage is not configured. Set Storage:PersonImagesPath in user-secrets.";
    }
}
