using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.Data.SqlClient;

namespace DVLD.Api.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DVLDConnection")
                ?? throw new InvalidOperationException("Connection string 'DVLDConnection' not found.");
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Users WHERE Username = @Username";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new UserDto
                {
                    UserID = (int)reader["UserID"],
                    PersonID = (int)reader["PersonID"],
                    Username = (string)reader["Username"],
                    Password = (string)reader["Password"],  
                    IsActive = (bool)reader["IsActive"]
                };
            }

            return null;
        }
        public async Task<bool> UpdatePasswordAsync(int userId, string newHashedPassword)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Password", newHashedPassword);
            command.Parameters.AddWithValue("@UserID", userId);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}