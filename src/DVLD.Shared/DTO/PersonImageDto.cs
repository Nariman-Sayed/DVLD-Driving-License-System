namespace DVLD.Shared
{
    /// <summary>
    /// Result of storing a person image.
    /// </summary>
    public sealed class PersonImageDto
    {
        /// <summary>
        /// Gets or sets the generated file name stored for the person.
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;
    }
}
