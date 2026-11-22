namespace DVLD.Api.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource does not exist.
    /// </summary>
    public sealed class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">The user-facing not found message.</param>
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
