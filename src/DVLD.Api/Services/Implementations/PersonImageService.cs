using DVLD.Api.Constants;
using DVLD.Api.Data.Interfaces;
using DVLD.Api.Exceptions;
using DVLD.Api.Options;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.Services.Implementations
{
    /// <summary>
    /// Stores person images under a configured absolute directory and keeps only the generated file name.
    /// </summary>
    internal sealed class PersonImageService(
        IPersonRepository repository,
        StorageOptions storageOptions,
        ILogger<PersonImageService> logger) : IPersonImageService
    {
        private static readonly byte[] JpegSignature = [0xFF, 0xD8, 0xFF];
        private static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];

        /// <inheritdoc />
        public async Task<PersonImageDto> UploadAsync(
            int personId,
            byte[] content,
            CancellationToken cancellationToken = default)
        {
            if (content.Length == 0 || content.Length > ImageRules.MaxBytes)
            {
                throw new ApplicationValidationException(ApiErrorMessages.InvalidImageFile);
            }

            var (extension, _) = DetectFormat(content);
            var directory = ResolveStorageDirectory(storageOptions);

            var existingPath = await repository.GetImagePathAsync(personId, cancellationToken);
            if (existingPath is null)
            {
                throw new NotFoundException(ApiErrorMessages.PersonNotFound);
            }

            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            await File.WriteAllBytesAsync(Path.Combine(directory, fileName), content, cancellationToken);

            if (!await repository.SetImagePathAsync(personId, fileName, cancellationToken))
            {
                throw new NotFoundException(ApiErrorMessages.PersonNotFound);
            }

            logger.LogInformation("Stored an image for person {PersonId}", personId);
            return new PersonImageDto { ImagePath = fileName };
        }

        /// <inheritdoc />
        public async Task<(byte[] Content, string ContentType)> DownloadAsync(
            int personId,
            CancellationToken cancellationToken = default)
        {
            var storedFileName = await repository.GetImagePathAsync(personId, cancellationToken);

            if (storedFileName is null)
            {
                throw new NotFoundException(ApiErrorMessages.PersonNotFound);
            }

            if (!IsSafeFileName(storedFileName))
            {
                throw new NotFoundException(ApiErrorMessages.PersonImageNotFound);
            }

            var directory = ResolveStorageDirectory(storageOptions);
            var fullPath = Path.Combine(directory, storedFileName);

            if (!File.Exists(fullPath))
            {
                throw new NotFoundException(ApiErrorMessages.PersonImageNotFound);
            }

            var content = await File.ReadAllBytesAsync(fullPath, cancellationToken);
            var (_, contentType) = DetectFormat(content);
            return (content, contentType);
        }

        private static (string Extension, string ContentType) DetectFormat(byte[] content)
        {
            if (content.Length >= JpegSignature.Length &&
                content.AsSpan(0, JpegSignature.Length).SequenceEqual(JpegSignature))
            {
                return (".jpg", "image/jpeg");
            }

            if (content.Length >= PngSignature.Length &&
                content.AsSpan(0, PngSignature.Length).SequenceEqual(PngSignature))
            {
                return (".png", "image/png");
            }

            throw new ApplicationValidationException(ApiErrorMessages.InvalidImageFile);
        }

        private static string ResolveStorageDirectory(StorageOptions options)
        {
            var configured = options.PersonImagesPath;

            if (string.IsNullOrWhiteSpace(configured) || !Path.IsPathRooted(configured))
            {
                throw new ImageStorageException();
            }

            return Path.GetFullPath(configured);
        }

        private static bool IsSafeFileName(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || Path.IsPathRooted(value))
            {
                return false;
            }

            if (value is "." or "..")
            {
                return false;
            }

            return string.Equals(value, Path.GetFileName(value), StringComparison.Ordinal);
        }
    }
}
