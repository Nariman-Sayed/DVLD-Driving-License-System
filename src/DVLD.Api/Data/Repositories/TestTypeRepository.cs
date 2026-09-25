using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.Data.SqlClient;

namespace DVLD.Api.Data.Repositories
{
    public class TestTypeRepository : ITestTypeRepository
    {
        private readonly string _connectionString;

        public TestTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DVLDConnection")
                ?? throw new InvalidOperationException("Connection string 'DVLDConnection' not found.");
        }

        public async Task<List<TestTypeDto>> GetAllAsync()
        {
            var types = new List<TestTypeDto>();
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM TestTypes ORDER BY TestTypeID";
            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                types.Add(MapReaderToDto(reader));
            }
            return types;
        }

        public async Task<TestTypeDto?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapReaderToDto(reader) : null;
        }

        public async Task<int> AddAsync(TestTypeDto type)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
                             VALUES (@TestTypeTitle, @TestTypeDescription, @TestTypeFees);
                             SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeTitle", type.TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", type.TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", type.TestTypeFees);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(int id, TestTypeDto type)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE TestTypes
                             SET TestTypeTitle = @TestTypeTitle,
                                 TestTypeDescription = @TestTypeDescription,
                                 TestTypeFees = @TestTypeFees
                             WHERE TestTypeID = @TestTypeID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", id);
            command.Parameters.AddWithValue("@TestTypeTitle", type.TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", type.TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", type.TestTypeFees);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static TestTypeDto MapReaderToDto(SqlDataReader reader)
        {
            return new TestTypeDto
            {
                TestTypeID = (int)reader["TestTypeID"],
                TestTypeTitle = (string)reader["TestTypeTitle"],
                TestTypeDescription = (string)reader["TestTypeDescription"],
                TestTypeFees = (decimal)reader["TestTypeFees"]
            };
        }
    }
}