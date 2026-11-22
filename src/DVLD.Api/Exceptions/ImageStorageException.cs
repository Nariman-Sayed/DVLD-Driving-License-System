using DVLD.Api.Constants;

namespace DVLD.Api.Exceptions
{
    /// <summary>
    /// Exception thrown when person image storage is not configured.
    /// </summary>
    public sealed class ImageStorageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageStorageException"/> class.
        /// </summary>
        public ImageStorageException() : base(ApiErrorMessages.ImageStorageNotConfigured)
        {
        }
    }
}
