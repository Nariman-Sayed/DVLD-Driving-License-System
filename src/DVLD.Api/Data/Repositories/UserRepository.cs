using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Repositories
{
    internal sealed class UserRepository(IApplicationDbContext context) : IUserRepository
    {
        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            return await context.Users
                .AsNoTracking()
                .Where(u => u.Username == username)
                .Select(u => new UserDto
                {
                    UserID = u.UserID,
                    PersonID = u.PersonID,
                    Username = u.Username,
                    Password = u.Password,
                    IsActive = u.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newHashedPassword)
        {
            var rowsAffected = await context.Users
                .Where(u => u.UserID == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.Password, newHashedPassword));

            return rowsAffected > 0;
        }
    }
}
