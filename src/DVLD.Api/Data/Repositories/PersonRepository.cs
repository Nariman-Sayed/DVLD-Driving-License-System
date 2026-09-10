using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.Data.SqlClient;

namespace DVLD.Api.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connectionString;

        public PersonRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DVLDConnection")
                ?? throw new InvalidOperationException("Connection string 'DVLDConnection' not found.");
        }

        public async Task<List<PersonDto>> GetAllAsync()
        {
            var people = new List<PersonDto>();
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM People ORDER BY FirstName";
            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                people.Add(MapReaderToPersonDto(reader));
            }
            return people;
        }

        public async Task<PersonDto?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapReaderToPersonDto(reader) : null;
        }

        public async Task<int> AddAsync(PersonDto person)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO People (FirstName, SecondName, ThirdName, LastName, NationalNo,
                                                   DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
                             VALUES (@FirstName, @SecondName, @ThirdName, @LastName, @NationalNo,
                                     @DateOfBirth, @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                             SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(query, connection);
            AddPersonParameters(command, person);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(int id, PersonDto person)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE People SET
                                FirstName = @FirstName, SecondName = @SecondName, ThirdName = @ThirdName,
                                LastName = @LastName, NationalNo = @NationalNo, DateOfBirth = @DateOfBirth,
                                Gendor = @Gendor, Address = @Address, Phone = @Phone, Email = @Email,
                                NationalityCountryID = @NationalityCountryID, ImagePath = @ImagePath
                              WHERE PersonID = @PersonID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", id);
            AddPersonParameters(command, person);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "DELETE FROM People WHERE PersonID = @PersonID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", id);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static PersonDto MapReaderToPersonDto(SqlDataReader reader)
        {
            return new PersonDto
            {
                PersonID = (int)reader["PersonID"],
                FirstName = (string)reader["FirstName"],
                SecondName = (string)reader["SecondName"],
                ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : "",
                LastName = (string)reader["LastName"],
                NationalNo = (string)reader["NationalNo"],
                DateOfBirth = (DateTime)reader["DateOfBirth"],
                Gendor = (short)(byte)reader["Gendor"],
                Address = (string)reader["Address"],
                Phone = (string)reader["Phone"],
                Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : "",
                NationalityCountryID = (int)reader["NationalityCountryID"],
                ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : ""
            };
        }

        private static void AddPersonParameters(SqlCommand command, PersonDto person)
        {
            command.Parameters.AddWithValue("@FirstName", person.FirstName);
            command.Parameters.AddWithValue("@SecondName", person.SecondName);
            command.Parameters.AddWithValue("@ThirdName",
                string.IsNullOrEmpty(person.ThirdName) ? DBNull.Value : person.ThirdName);
            command.Parameters.AddWithValue("@LastName", person.LastName);
            command.Parameters.AddWithValue("@NationalNo", person.NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", person.Gendor);
            command.Parameters.AddWithValue("@Address", person.Address);
            command.Parameters.AddWithValue("@Phone", person.Phone);
            command.Parameters.AddWithValue("@Email",
                string.IsNullOrEmpty(person.Email) ? DBNull.Value : person.Email);
            command.Parameters.AddWithValue("@NationalityCountryID", person.NationalityCountryID);
            command.Parameters.AddWithValue("@ImagePath",
                string.IsNullOrEmpty(person.ImagePath) ? DBNull.Value : person.ImagePath);
        }
    }
}