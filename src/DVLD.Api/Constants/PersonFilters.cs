namespace DVLD.Api.Constants
{
    /// <summary>
    /// The only filter fields accepted by the person list endpoint.
    /// </summary>
    public static class PersonFilters
    {
        /// <summary>
        /// The national number filter field.
        /// </summary>
        public const string NationalNo = "nationalno";

        /// <summary>
        /// The first name filter field.
        /// </summary>
        public const string FirstName = "firstname";

        /// <summary>
        /// The last name filter field.
        /// </summary>
        public const string LastName = "lastname";

        /// <summary>
        /// The country identifier filter field.
        /// </summary>
        public const string CountryId = "countryid";

        /// <summary>
        /// Case-insensitive set of the supported filter names.
        /// </summary>
        public static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
        {
            NationalNo,
            FirstName,
            LastName,
            CountryId
        };
    }
}
