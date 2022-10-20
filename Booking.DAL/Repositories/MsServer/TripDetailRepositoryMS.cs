using System;
using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class TripDetailRepositoryMS : ITripDetailRepository
    {
        private readonly string _connectionString;

        public TripDetailRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(TripDetails entity)
        {
            string query = $"INSERT INTO TripDetails (UserId,TripId,Place) VALUES(@UserId,@TripId,@Place)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@TripId", SqlDbType.Int).Value = entity.TripId;
                    sqlCommand.Parameters.Add("@Place", SqlDbType.Int).Value = entity.Place;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM TripDetails WHERE Id = @id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> DeleteByPlace(int tripId, int place)
        {
            string query = $@"DELETE FROM TripDetails WHERE TripId = @tripId AND Place=@Place";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@tripId", SqlDbType.Int).Value = tripId;
                    sqlCommand.Parameters.Add("@Place", SqlDbType.Int).Value = place;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<List<TripDetails>> GetAll()
        {
            string sql = $"select Id,UserId,TripId,Place from TripDetails";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    var data = await GetTripDetails(sqlCommand);

                    return data;
                
                }
            }
        }

        public async Task<List<TripDetails>> GetDetailsByTripId(int tripId)
        {
            string sql = $"select Id,UserId,TripId,Place from TripDetails where TripId=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = tripId;

                    var data = await GetTripDetails(sqlCommand);

                    return data;
                }
            }
        }

        public async Task<TripDetails> GetById(int id)
        {
            string sql = $"select Id,UserId,TripId,Place from TripDetails where Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    SqlParameter username = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = id,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input
                    };

                    sqlCommand.Parameters.Add(username);

                    var data= await GetTripDetails(sqlCommand);

                    return data.FirstOrDefault();
                }

            }
        }

        public async Task<bool> Update(TripDetails entity)
        {
            string query = $"UPDATE TripDetails SET UserId=@UserId,TripId=@TripId,Place=@Place WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@TripId", SqlDbType.Int).Value = entity.TripId;
                    sqlCommand.Parameters.Add("@Place", SqlDbType.Int).Value = entity.Place;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        private async Task<List<TripDetails>> GetTripDetails(SqlCommand sqlCommand)
        {
            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
            {
                List<TripDetails> tripDetails = new List<TripDetails>();

                while (await sqlDataReader.ReadAsync())
                {
                    tripDetails.Add(new TripDetails()
                    {
                        Id = (int)sqlDataReader["Id"],
                        UserId = (int)sqlDataReader["UserId"],
                        TripId = (int)sqlDataReader["TripId"],
                        Place = (int)sqlDataReader["Place"]

                    });
                }
                if (tripDetails.Count > 0)
                {
                    return tripDetails;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
