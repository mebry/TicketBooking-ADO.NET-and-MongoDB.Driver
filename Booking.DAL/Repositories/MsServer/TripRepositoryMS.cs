using System;
using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class TripRepositoryMS : ITripRepository
    {
        private readonly string _connectionString;

        public TripRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(Trip entity)
        {
            string query = $"INSERT INTO Trips (PlaneId,StartCityId,EndCityId,StartDate,EndDate,Price,Capacity) VALUES(@PlaneId,@StartCityId," +
                $"@EndCityId,@StartDate,@EndDate,@Price,@Capacity)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@PlaneId", SqlDbType.Int).Value = entity.PlaneId;
                    sqlCommand.Parameters.Add("@StartCityId", SqlDbType.Int).Value = entity.StartCityId;
                    sqlCommand.Parameters.Add("@EndCityId", SqlDbType.Int).Value = entity.EndCityId;
                    sqlCommand.Parameters.Add("@Price", SqlDbType.Int).Value = entity.Price;
                    sqlCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = entity.StartDate;
                    sqlCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = entity.EndDate;
                    sqlCommand.Parameters.Add("@Capacity", SqlDbType.Int).Value = entity.Capacity;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM Trips WHERE Id = @id";

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

        public async Task<List<Trip>> GetAll()
        {
            string sql = $"select Id,PlaneId,StartCityId,EndCityId,StartDate,EndDate,Price,Capacity from Trips";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<Trip> trips = new List<Trip>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            trips.Add(new Trip()
                            {
                                Id = (int)sqlDataReader["Id"],
                                PlaneId = (int)sqlDataReader["PlaneId"],
                                Capacity = (int)sqlDataReader["Capacity"],
                                Price = (int)sqlDataReader["Price"],
                                StartCityId = (int)sqlDataReader["StartCityId"],
                                EndCityId = (int)sqlDataReader["EndCityId"],
                                StartDate = (DateTime)sqlDataReader["StartDate"],
                                EndDate = (DateTime)sqlDataReader["EndDate"]
                            });
                        }
                        if (trips.Count > 0)
                        {
                            return trips;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<Trip> GetById(int id)
        {
            string sql = $"select Id,PlaneId,StartCityId,EndCityId,StartDate,EndDate,Price,Capacity from Trips where Id=@id";

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

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (await sqlDataReader.ReadAsync())
                        {
                            return new Trip()
                            {
                                Id = (int)sqlDataReader["Id"],
                                PlaneId = (int)sqlDataReader["PlaneId"],
                                Capacity = (int)sqlDataReader["Capacity"],
                                Price = (int)sqlDataReader["Price"],
                                StartCityId = (int)sqlDataReader["StartCityId"],
                                EndCityId = (int)sqlDataReader["EndCityId"],
                                StartDate = (DateTime)sqlDataReader["StartDate"],
                                EndDate = (DateTime)sqlDataReader["EndDate"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> Update(Trip entity)
        {
            string query = $"UPDATE Roles SET PlaneId=@PlaneId,StartCityId=@StartCityId," +
                $"EndCityId=@EndCityId,StartDate=@StartDate,EndDate=@EndDate," +
                $"Price=@Price,Capacity=@Capacity WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@PlaneId", SqlDbType.Int).Value = entity.PlaneId;
                    sqlCommand.Parameters.Add("@StartCityId", SqlDbType.Int).Value = entity.StartCityId;
                    sqlCommand.Parameters.Add("@EndCityId", SqlDbType.Int).Value = entity.EndCityId;
                    sqlCommand.Parameters.Add("@Price", SqlDbType.Int).Value = entity.Price;
                    sqlCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = entity.StartDate;
                    sqlCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = entity.EndDate;
                    sqlCommand.Parameters.Add("@Capacity", SqlDbType.Int).Value = entity.Capacity;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}
