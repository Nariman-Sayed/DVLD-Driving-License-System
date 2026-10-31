namespace DVLD.Api.Exceptions
{
    /// <summary>
    /// Exception thrown when an application request fails validation.
    /// </summary>
    public class ApplicationValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationValidationException"/> class.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        public ApplicationValidationException(string message) : base(message)
        {
        }
    }
}
