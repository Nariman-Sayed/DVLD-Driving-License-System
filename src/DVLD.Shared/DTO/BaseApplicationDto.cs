namespace DVLD.Shared
{
    /// <summary>
    /// Data transfer object for a base application.
    /// </summary>
    public class BaseApplicationDto
    {
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        public int ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the applicant person identifier.
        /// </summary>
        public int ApplicantPersonID { get; set; }

        /// <summary>
        /// Gets or sets the application date.
        /// </summary>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// Gets or sets the application type identifier.
        /// </summary>
        public int ApplicationTypeID { get; set; }

        /// <summary>
        /// Gets or sets the application status.
        /// </summary>
        public byte ApplicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the last status date.
        /// </summary>
        public DateTime LastStatusDate { get; set; }

        /// <summary>
        /// Gets or sets the paid fees.
        /// </summary>
        public decimal PaidFees { get; set; }

        /// <summary>
        /// Gets or sets the created-by user identifier.
        /// </summary>
        public int CreatedByUserID { get; set; }
    }
}
