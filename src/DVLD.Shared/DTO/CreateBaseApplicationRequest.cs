namespace DVLD.Shared
{
    /// <summary>
    /// Request payload for creating a base application.
    /// </summary>
    public class CreateBaseApplicationRequest
    {
        /// <summary>
        /// Gets or sets the applicant person identifier.
        /// </summary>
        public int ApplicantPersonID { get; set; }

        /// <summary>
        /// Gets or sets the application type identifier.
        /// </summary>
        public int ApplicationTypeID { get; set; }
    }
}
