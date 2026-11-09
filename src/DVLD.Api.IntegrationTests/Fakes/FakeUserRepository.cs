using DVLD.Api.Data.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.IntegrationTests.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        public const string ActiveUsername = "active-user";
        public const string InactiveUsername = "inactive-user";
        public const string PlaintextUsername = "plaintext-user";
        public const string ValidPassword = "CorrectPassword!1";
        public const string PlaintextPassword = "PlainTextPassword!1";

        // Work factor 4 keeps the test host fast; verification works with any cost.
        private static readonly string HashedPassword = BCrypt.Net.BCrypt.HashPassword(ValidPassword, 4);

        private readonly List<UserDto> _users = new()
        {
            new UserDto
            {
                UserID = 1,
                PersonID = 1,
                Username = ActiveUsername,
                Password = HashedPassword,
                IsActive = true
            },
            new UserDto
            {
                UserID = 2,
                PersonID = 2,
                Username = InactiveUsername,
                Password = HashedPassword,
                IsActive = false
            },
            new UserDto
            {
                UserID = 3,
                PersonID = 3,
                Username = PlaintextUsername,
                Password = PlaintextPassword,
                IsActive = true
            }
        };

        public Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            return Task.FromResult(user);
        }

        public Task<bool> UpdatePasswordAsync(int userId, string newHashedPassword)
        {
            var user = _users.FirstOrDefault(u => u.UserID == userId);
            if (user is null)
            {
                return Task.FromResult(false);
            }

            user.Password = newHashedPassword;
            return Task.FromResult(true);
        }
    }
}
