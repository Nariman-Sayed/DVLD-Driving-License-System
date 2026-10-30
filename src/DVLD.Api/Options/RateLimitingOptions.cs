using System.ComponentModel.DataAnnotations;

namespace DVLD.Api.Options
{
    /// <summary>
    /// Strongly typed rate limiting configuration bound from the <c>RateLimiting</c> section.
    /// </summary>
    public sealed class RateLimitingOptions
    {
        /// <summary>
        /// Gets or sets the permit limit per window.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PermitLimit { get; set; } = 100;

        /// <summary>
        /// Gets or sets the window size in seconds.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int WindowSeconds { get; set; } = 60;

        /// <summary>
        /// Gets or sets the queue limit.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int QueueLimit { get; set; } = 0;
    }
}
