namespace DVLD.Api.Data.Entities
{
    internal sealed class TestType
    {
        public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; } = string.Empty;
        public string TestTypeDescription { get; set; } = string.Empty;
        public decimal TestTypeFees { get; set; }
    }
}
