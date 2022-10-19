using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class CountryRepositoryMS : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(Country entity)
        {
            string query = $"INSERT INTO Countries (Name) VALUES(@Name)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM Countries WHERE Countries.Id = @id";

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

        public async Task<List<Country>> GetAll()
        {
            string sql = $"select Id, Name from Countries";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<Country> countries = new List<Country>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            countries.Add(new Country()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined"

                            });
                        }
                        if (countries.Count > 0)
                        {
                            return countries;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<List<City>> GetAllCities(int countryId)
        {
            string sql = $@"SELECT Cities.Id,CountryId,Cities.Name
                        FROM Cities
                        WHERE Cities.CountryId=@countryId";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@countryId", SqlDbType.Int).Value = countryId;
                    List<City> countries = new List<City>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            countries.Add(new City()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined",
                                CountryId = (int)sqlDataReader["CountryId"]

                            });
                        }
                        if (countries.Count > 0)
                        {
                            return countries;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

        }

        public async Task<Country> GetById(int id)
        {
            string sql = $"select Id, Name from Countries where Id=@id";

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
                            return new Country()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined"

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<Country> GetByName(string name)
        {
            string sql = $"select Id, Name from Countries where Name=@Name";

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
                            return new Country()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined"

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<bool> Update(Country entity)
        {
            string query = $"UPDATE Countries SET Name=@Name WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}