using System;
using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class UserRoleRepositoryMS : IUserRoleRepository
    {
        private readonly string _connectionString;

        public UserRoleRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(UserRole entity)
        {
            string query = $"INSERT INTO UserRoles (UserId,RoleId) VALUES(@UserId,@RoleId)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@RoleId", SqlDbType.Int).Value = entity.RoleId;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM UserRoles WHERE Id = @id";

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

        public async Task<List<UserRole>> GetAll()
        {
            string sql = $"select Id,UserId,RoleId from UserRoles";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<UserRole> roles = new List<UserRole>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            roles.Add(new UserRole()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserId = (int)sqlDataReader["UserId"],
                                RoleId = (int)sqlDataReader["RoleId"]

                            });
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

        public async Task<UserRole> GetById(int id)
        {
            string sql = $"select Id,UserId,RoleId from UserRoles where Id=@id";

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
                            return new UserRole()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserId = (int)sqlDataReader["UserId"],
                                RoleId = (int)sqlDataReader["RoleId"]

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<List<UserRole>> GetByUserName(string username)
        {
            string sql = $"select Users.Id,Users.UserId,Users.RoleId from UserRoles,Users where Users.Id=@id AND Users.UserName = @UserName";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    SqlParameter username1 = new SqlParameter
                    {
                        ParameterName = "@UserName",
                        Value = username,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };

                    sqlCommand.Parameters.Add(username1);
                    List<UserRole> roles = new List<UserRole>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            roles.Add(new UserRole()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserId = (int)sqlDataReader["UserId"],
                                RoleId = (int)sqlDataReader["RoleId"]

                            });
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

        public async Task<bool> Update(UserRole entity)
        {
            string query = $"UPDATE Roles SET UserId=@UserId,RoleId=@RoleId WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@RoleId", SqlDbType.Int).Value = entity.RoleId;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}
