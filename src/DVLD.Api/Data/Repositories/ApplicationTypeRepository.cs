using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.Data.SqlClient;

namespace DVLD.Api.Data.Repositories
{
    public class ApplicationTypeRepository : IApplicationTypeRepository
    {
        private readonly string _connectionString;

        public ApplicationTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DVLDConnection")
                ?? throw new InvalidOperationException("Connection string 'DVLDConnection' not found.");
        }

        public async Task<List<ApplicationTypeDto>> GetAllAsync()
        {
            var types = new List<ApplicationTypeDto>();
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM ApplicationTypes ORDER BY ApplicationTypeTitle";
            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                types.Add(MapReaderToDto(reader));
            }
            return types;
        }

        public async Task<ApplicationTypeDto?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapReaderToDto(reader) : null;
        }

        public async Task<int> AddAsync(ApplicationTypeDto type)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees)
                             VALUES (@ApplicationTypeTitle, @ApplicationFees);
                             SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", type.ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", type.ApplicationFees);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(int id, ApplicationTypeDto type)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE ApplicationTypes SET
                                ApplicationTypeTitle = @ApplicationTypeTitle,
                                ApplicationFees = @ApplicationFees
                              WHERE ApplicationTypeID = @ApplicationTypeID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", id);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", type.ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", type.ApplicationFees);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static ApplicationTypeDto MapReaderToDto(SqlDataReader reader)
        {
            return new ApplicationTypeDto
            {
                ApplicationTypeID = (int)reader["ApplicationTypeID"],
                ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"],
                ApplicationFees = (decimal)reader["ApplicationFees"]
            };
        }
    }
}