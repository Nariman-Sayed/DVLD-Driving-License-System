using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DVLD.Api.Options
{
    /// <summary>
    /// Strongly typed JWT configuration bound from the <c>Jwt</c> section.
    /// </summary>
    public sealed class JwtOptions
    {
        /// <summary>
        /// Gets or sets the symmetric signing key. Must be at least 32 bytes.
        /// </summary>
        [Required(ErrorMessage = "JWT key is missing. Set Jwt:Key in configuration or user-secrets.")]
        [MinimumBytes(32, ErrorMessage = "JWT key must be at least 32 bytes.")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token issuer.
        /// </summary>
        [Required(ErrorMessage = "JWT issuer is missing. Set Jwt:Issuer in configuration.")]
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token audience.
        /// </summary>
        [Required(ErrorMessage = "JWT audience is missing. Set Jwt:Audience in configuration.")]
        public string Audience { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validation attribute ensuring a string is at least the specified number of UTF-8 bytes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class MinimumBytesAttribute : ValidationAttribute
    {
        private readonly int _minimumBytes;

        public MinimumBytesAttribute(int minimumBytes)
        {
            _minimumBytes = minimumBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string text && Encoding.UTF8.GetByteCount(text) < _minimumBytes)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
