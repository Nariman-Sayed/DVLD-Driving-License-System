using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    /// <summary>
    /// Application authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Validates credentials and issues a JWT token on success.
        /// </summary>
        /// <param name="request">The login credentials.</param>
        /// <returns>The token and user details, or <see langword="null"/> when the credentials are invalid or the account is inactive.</returns>
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
