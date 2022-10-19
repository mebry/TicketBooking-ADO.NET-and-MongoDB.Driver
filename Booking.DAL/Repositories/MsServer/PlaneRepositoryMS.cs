using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class PlaneRepositoryMS : IPlaneRepository
    {
        private readonly string _connectionString;

        public PlaneRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(Plane entity)
        {
            string query = $"INSERT INTO Planes (Name, Capacity) VALUES(@Name,@Capacity)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCommand.Parameters.Add("@Capacity", SqlDbType.NVarChar).Value = entity.Capacity;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM Planes WHERE Planes.Id = @id";

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

        public async Task<List<Plane>> GetAll()
        {
            string sql = $"select Id, Name,Capacity from Planes";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<Plane> planes = new List<Plane>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            planes.Add(new Plane()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined",
                                Capacity = (int)sqlDataReader["Capacity"]

                            });
                        }
                        if (planes.Count > 0)
                        {
                            return planes;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<Plane> GetById(int id)
        {
            string sql = $"select Id, Name,Capacity from Planes where Id=@id";

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
                            return new Plane()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined",
                                Capacity = (int)sqlDataReader["Capacity"]

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<Plane> GetByPlaneName(string planeName)
        {
            string sql = $"select Id, Name,Capacity from Planes where Name=@Name";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    SqlParameter username = new SqlParameter
                    {
                        ParameterName = "@Name",
                        Value = planeName,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };

                    sqlCommand.Parameters.Add(username);

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (await sqlDataReader.ReadAsync())
                        {
                            return new Plane()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined",
                                Capacity = (int)sqlDataReader["Capacity"]

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<bool> Update(Plane entity)
        {
            string query = $"UPDATE Planes SET Capacity=@Capacity,Name=@Name WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCommand.Parameters.Add("@Capacity", SqlDbType.Int).Value = entity.Capacity;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}

