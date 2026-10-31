namespace DVLD.Api.Data.Entities
{
    internal sealed class ApplicationType
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; } = string.Empty;
        public decimal ApplicationFees { get; set; }
    }
}
