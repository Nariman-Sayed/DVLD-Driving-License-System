namespace DVLD.Shared
{
    /// <summary>
    /// Data transfer object for a test type.
    /// </summary>
    public class TestTypeDto
    {
        /// <summary>
        /// Gets or sets the test type identifier.
        /// </summary>
        public int TestTypeID { get; set; }

        /// <summary>
        /// Gets or sets the test type title.
        /// </summary>
        public string TestTypeTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the test type description.
        /// </summary>
        public string TestTypeDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the test type fees.
        /// </summary>
        public decimal TestTypeFees { get; set; }
    }
}
