namespace DVLD.Shared
{
    /// <summary>
    /// Data transfer object for a country.
    /// </summary>
    public class CountryDto
    {
        /// <summary>
        /// Gets or sets the country identifier.
        /// </summary>
        public int CountryID { get; set; }

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        public string CountryName { get; set; } = string.Empty;
    }
}
