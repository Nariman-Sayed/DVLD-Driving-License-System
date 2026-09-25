using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.Data.SqlClient;

namespace DVLD.Api.Data.Repositories
{
    public class BaseApplicationRepository : IBaseApplicationRepository
    {
        private readonly string _connectionString;

        public BaseApplicationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DVLDConnection")
                ?? throw new InvalidOperationException("Connection string 'DVLDConnection' not found.");
        }

        public async Task<List<BaseApplicationDto>> GetAllAsync()
        {
            var applications = new List<BaseApplicationDto>();
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Applications ORDER BY ApplicationDate DESC";
            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                applications.Add(MapReaderToDto(reader));
            }
            return applications;
        }

        public async Task<BaseApplicationDto?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapReaderToDto(reader) : null;
        }

        public async Task<int> AddAsync(BaseApplicationDto application)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO Applications
                             (ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                              ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                             VALUES
                             (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID,
                              @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", application.ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", application.ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", application.ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", application.ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", application.LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", application.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", application.CreatedByUserID);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(int id, BaseApplicationDto application)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"UPDATE Applications
                             SET ApplicantPersonID = @ApplicantPersonID,
                                 ApplicationDate = @ApplicationDate,
                                 ApplicationTypeID = @ApplicationTypeID,
                                 ApplicationStatus = @ApplicationStatus,
                                 LastStatusDate = @LastStatusDate,
                                 PaidFees = @PaidFees,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE ApplicationID = @ApplicationID";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", id);
            command.Parameters.AddWithValue("@ApplicantPersonID", application.ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", application.ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", application.ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", application.ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", application.LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", application.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", application.CreatedByUserID);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static BaseApplicationDto MapReaderToDto(SqlDataReader reader)
        {
            return new BaseApplicationDto
            {
                ApplicationID = (int)reader["ApplicationID"],
                ApplicantPersonID = (int)reader["ApplicantPersonID"],
                ApplicationDate = (DateTime)reader["ApplicationDate"],
                ApplicationTypeID = (int)reader["ApplicationTypeID"],
                ApplicationStatus = (byte)reader["ApplicationStatus"],
                LastStatusDate = (DateTime)reader["LastStatusDate"],
                PaidFees = (decimal)reader["PaidFees"],
                CreatedByUserID = (int)reader["CreatedByUserID"]
            };
        }
    }
}