using DVLD.Api.Data.Interfaces;
using DVLD.Api.Services.Interfaces;
using DVLD.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DVLD.Api.Services.Implementations
{
    /// <summary>
    /// Authenticates users against the stored password hashes and issues JWT tokens.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The user data access.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The logger.</param>
        public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Validates credentials and issues a JWT token on success.
        /// </summary>
        /// <param name="request">The login credentials.</param>
        /// <returns>The token and user details, or <see langword="null"/> when the credentials are invalid or the account is inactive.</returns>
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInformation("Login attempt for username: {Username}", request.Username);

            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user is null)
            {
                _logger.LogWarning("Login failed: username {Username} not found", request.Username);
                return null;
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed: account {Username} is not active", request.Username);
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed: incorrect password for username {Username}", request.Username);
                return null;
            }

            string token = GenerateJwtToken(user);

            _logger.LogInformation("Login successful for username: {Username}", request.Username);

            return new LoginResponseDto
            {
                Token = token,
                UserID = user.UserID,
                Username = user.Username
            };
        }

        private string GenerateJwtToken(UserDto user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}