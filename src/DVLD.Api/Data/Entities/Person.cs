namespace DVLD.Api.Data.Entities
{
    internal sealed class Person
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string? ThirdName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string? ImagePath { get; set; }
    }
}
