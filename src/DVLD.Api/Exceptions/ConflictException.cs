namespace DVLD.Api.Exceptions
{
    /// <summary>
    /// Exception thrown when a request conflicts with the current state of the data.
    /// </summary>
    public sealed class ConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class.
        /// </summary>
        /// <param name="message">The user-facing conflict message.</param>
        public ConflictException(string message) : base(message)
        {
        }
    }
}
