using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<bool> UpdatePasswordAsync(int userId, string newHashedPassword);
    }
}