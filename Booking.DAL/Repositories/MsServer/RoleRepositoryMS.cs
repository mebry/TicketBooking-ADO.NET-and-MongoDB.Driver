using System;
using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class RoleRepositoryMS : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(Role entity)
        {
            string query = $"INSERT INTO Roles (Name) VALUES(@Name)";

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
            string query = $@"DELETE FROM Roles WHERE Roles.Id = @id";

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

        public async Task<List<Role>> GetAll()
        {
            string sql = $"select Id, Name from Roles";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<Role> roles = new List<Role>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            roles.Add(new Role()
                            {
                                Id = (int)sqlDataReader["Id"],
                                Name = sqlDataReader["Name"] as string ?? "Undefined"

                            }); ;
                        }
                        if (roles.Count > 0)
                        {
                            return roles;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<Role> GetById(int id)
        {
            string sql = $"select Id, Name from Roles where Id=@id";

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
                            return new Role()
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

        public async Task<Role> GetByName(string name)
        {
            string sql = $"select Id, Name from Roles where Name=@Name";

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
                            return new Role()
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

        public async Task<bool> Update(Role entity)
        {
            string query = $"UPDATE Roles SET Name=@Name WHERE Id=@id";

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
