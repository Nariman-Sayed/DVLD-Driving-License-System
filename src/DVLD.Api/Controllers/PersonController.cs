using DVLD.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IConfiguration configuration, ILogger<PersonController> logger)
        {
            _connectionString = configuration.GetConnectionString("DVLDConnection")
                ?? throw new InvalidOperationException("Connection string 'DVLDConnection' not found.");
            _logger = logger;
        }

        // GET: api/person
        [HttpGet]
        public ActionResult<List<PersonDto>> GetAllPeople()
        {
            _logger.LogInformation("Fetching all people");

            var people = new List<PersonDto>();

            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM People ORDER BY FirstName";
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                people.Add(MapReaderToPersonDto(reader));
            }

            _logger.LogInformation("Successfully retrieved {Count} people", people.Count);
            return Ok(people);
        }

        // GET: api/person/5
        [HttpGet("{id}")]
        public ActionResult<PersonDto> GetPersonById(int id)
        {
            _logger.LogInformation("Fetching person with ID: {PersonId}", id);

            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", id);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var person = MapReaderToPersonDto(reader);
                _logger.LogInformation("Found person with ID: {PersonId}", id);
                return Ok(person);
            }

            _logger.LogWarning("Person with ID {PersonId} not found", id);
            return NotFound($"Person with ID {id} not found.");
        }

        // POST: api/person
        [HttpPost]
        public ActionResult<PersonDto> AddNewPerson(PersonDto person)
        {
            _logger.LogInformation("Attempting to add new person with National No: {NationalNo}", person.NationalNo);

            using var connection = new SqlConnection(_connectionString);

            string query = @"INSERT INTO People (FirstName, SecondName, ThirdName, LastName, NationalNo,
                                                   DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
                             VALUES (@FirstName, @SecondName, @ThirdName, @LastName, @NationalNo,
                                     @DateOfBirth, @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                             SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(query, connection);
            AddPersonParameters(command, person);

            connection.Open();
            var result = command.ExecuteScalar();
            person.PersonID = Convert.ToInt32(result);

            _logger.LogInformation("Successfully added person with ID: {PersonId}", person.PersonID);
            return CreatedAtAction(nameof(GetPersonById), new { id = person.PersonID }, person);
        }

        // PUT: api/person/5
        [HttpPut("{id}")]
        public ActionResult UpdatePerson(int id, PersonDto person)
        {
            _logger.LogInformation("Attempting to update person with ID: {PersonId}", id);

            using var connection = new SqlConnection(_connectionString);

            string query = @"UPDATE People SET
                                FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName,
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gendor = @Gendor,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath = @ImagePath
                              WHERE PersonID = @PersonID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", id);
            AddPersonParameters(command, person);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                _logger.LogWarning("Update failed: Person with ID {PersonId} not found", id);
                return NotFound($"Person with ID {id} not found.");
            }

            _logger.LogInformation("Successfully updated person with ID: {PersonId}", id);
            return NoContent();
        }

        // DELETE: api/person/5
        [HttpDelete("{id}")]
        public ActionResult DeletePerson(int id)
        {
            _logger.LogInformation("Attempting to delete person with ID: {PersonId}", id);

            using var connection = new SqlConnection(_connectionString);
            string query = "DELETE FROM People WHERE PersonID = @PersonID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", id);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                _logger.LogWarning("Delete failed: Person with ID {PersonId} not found", id);
                return NotFound($"Person with ID {id} not found.");
            }

            _logger.LogInformation("Successfully deleted person with ID: {PersonId}", id);
            return NoContent();
        }

        // Helper: maps a SqlDataReader row to a PersonDto (handles nullable columns)
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

        // Helper: adds all PersonDto fields as SqlCommand parameters (handles nullable columns)
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