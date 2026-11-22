namespace DVLD.Api.Options
{
    /// <summary>
    /// Configuration for file storage used by the API.
    /// </summary>
    public sealed class StorageOptions
    {
        /// <summary>
        /// The configuration section that holds storage settings.
        /// </summary>
        public const string SectionName = "Storage";

        /// <summary>
        /// Gets or sets the absolute directory where person images are stored.
        /// </summary>
        public string PersonImagesPath { get; set; } = string.Empty;
    }
}
