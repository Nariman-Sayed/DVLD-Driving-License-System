using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    /// <summary>
    /// Data access for user accounts.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Finds a user account by username.
        /// </summary>
        /// <param name="username">The username to look up.</param>
        /// <returns>The matching user, or <see langword="null"/> when no account exists.</returns>
        Task<UserDto?> GetByUsernameAsync(string username);

        /// <summary>
        /// Replaces the stored password hash for a user.
        /// </summary>
        /// <param name="userId">The identifier of the user to update.</param>
        /// <param name="newHashedPassword">The new password hash.</param>
        /// <returns><see langword="true"/> when a row was updated; otherwise <see langword="false"/>.</returns>
        Task<bool> UpdatePasswordAsync(int userId, string newHashedPassword);
    }
}
