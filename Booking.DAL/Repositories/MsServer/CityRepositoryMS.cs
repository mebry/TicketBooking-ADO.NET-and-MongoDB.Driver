using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;

namespace Booking.DAL.Repositories.MsServer
{
    public class CityRepositoryMS : ICityRepository
    {
        private readonly string _connectionString;

        public CityRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(City entity)
        {
            string query = $"INSERT INTO Cities (Name,CountryId) VALUES(@Name,@CountryId)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCommand.Parameters.Add("@CountryId", SqlDbType.Int).Value = entity.CountryId;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM Cities WHERE Id = @id";

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

        public async Task<List<City>> GetAll()
        {
            string sql = $"select Id, Name, CountryId  from Cities";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<City> cities = new List<City>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            cities.Add(new City()
                            {
                                Id = (int)sqlDataReader["Id"],
                                CountryId = (int)sqlDataReader["CountryId"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined"

                            });
                        }
                        if (cities.Count > 0)
                        {
                            return cities;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<City> GetById(int id)
        {
            string sql = $"select Id, Name, CountryId from Cities where Id=@id";

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
                            return new City()
                            {
                                Id = (int)sqlDataReader["Id"],
                                CountryId = (int)sqlDataReader["CountryId"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined"

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<City> GetByName(string name)
        {
            string sql = $"select Id, Name, CountryId from Cities where Name=@Name";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    SqlParameter username = new SqlParameter
                    {
                        ParameterName = "@Name",
                        Value = name,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };

                    sqlCommand.Parameters.Add(username);

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (await sqlDataReader.ReadAsync())
                        {
                            return new City()
                            {
                                Id = (int)sqlDataReader["Id"],
                                CountryId = (int)sqlDataReader["CountryId"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined"

                            };
                        }
                    }
                }

            }
            return null;
        }

        public async Task<bool> Update(City entity)
        {
            string query = $"UPDATE Cities SET Name=@Name,CountryId=@CountryId WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCommand.Parameters.Add("@CountryId", SqlDbType.Int).Value = entity.CountryId;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}
