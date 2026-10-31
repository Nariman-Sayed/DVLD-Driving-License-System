namespace DVLD.Shared
{
    /// <summary>
    /// Data transfer object for an application type.
    /// </summary>
    public class ApplicationTypeDto
    {
        /// <summary>
        /// Gets or sets the application type identifier.
        /// </summary>
        public int ApplicationTypeID { get; set; }

        /// <summary>
        /// Gets or sets the application type title.
        /// </summary>
        public string ApplicationTypeTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the application fees.
        /// </summary>
        public decimal ApplicationFees { get; set; }
    }
}
