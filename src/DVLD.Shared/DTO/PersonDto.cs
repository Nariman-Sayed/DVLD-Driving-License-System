using System.ComponentModel.DataAnnotations;

namespace DVLD.Shared
{
    /// <summary>
    /// Data transfer object for a person.
    /// </summary>
    public class PersonDto
    {
        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        public int PersonID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the second name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1)]
        public string SecondName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional third name.
        /// </summary>
        [StringLength(20)]
        public string ThirdName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the national number.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1)]
        public string NationalNo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the gender code using the database column spelling.
        /// </summary>
        [Required]
        public short Gendor { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(500, MinimumLength = 1)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 1)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional email address.
        /// </summary>
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the nationality country identifier.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int NationalityCountryID { get; set; }

        /// <summary>
        /// Gets or sets the stored image file name.
        /// </summary>
        [StringLength(250)]
        public string ImagePath { get; set; } = string.Empty;
    }
}
