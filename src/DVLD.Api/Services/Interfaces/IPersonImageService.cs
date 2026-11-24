using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    /// <summary>
    /// Application operations for storing and reading person images.
    /// </summary>
    public interface IPersonImageService
    {
        /// <summary>
        /// Validates and stores an image for a person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="content">The raw uploaded bytes.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The generated file name.</returns>
        /// <exception cref="DVLD.Api.Exceptions.ApplicationValidationException">The bytes are too large or are not JPEG or PNG.</exception>
        /// <exception cref="DVLD.Api.Exceptions.NotFoundException">No person exists with the given identifier.</exception>
        /// <exception cref="DVLD.Api.Exceptions.ImageStorageException">Image storage is not configured.</exception>
        Task<PersonImageDto> UploadAsync(int personId, byte[] content, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads a stored image for a person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The image bytes and the content type detected from the file signature.</returns>
        /// <exception cref="DVLD.Api.Exceptions.NotFoundException">The person or image does not exist.</exception>
        Task<(byte[] Content, string ContentType)> DownloadAsync(int personId, CancellationToken cancellationToken = default);
    }
}
