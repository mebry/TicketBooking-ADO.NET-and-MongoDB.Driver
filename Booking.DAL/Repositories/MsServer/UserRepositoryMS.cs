using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class UserRepositoryMS : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(User entity)
        {
            string query = $"INSERT INTO Users (UserName, Password,Codeword) VALUES(@username,@password,@codeword)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar).Value = entity.UserName;
                    sqlCommand.Parameters.Add("@password", SqlDbType.NVarChar).Value = entity.Password;
                    sqlCommand.Parameters.Add("@codeword", SqlDbType.NVarChar).Value = entity.Codeword;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM Users WHERE Users.Id = @id";

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

        public async Task<List<User>> GetAll()
        {
            string sql = $"select Id, UserName,Password,Codeword from Users";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<User> users = new List<User>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            users.Add(new User()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserName = sqlDataReader["UserName"] as string ?? "Undefined",
                                Password = sqlDataReader["Password"] as string ?? "Undefined",
                                Codeword = sqlDataReader["Codeword"] as string ?? "Undefined",

                            });
                        }
                        if (users.Count > 0)
                        {
                            return users;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<User> GetById(int id)
        {
            string sql = $"select Id, UserName,Password,Codeword from Users where Id=@id";

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
                            return new User()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserName = sqlDataReader["UserName"] as string ?? "Undefined",
                                Password = sqlDataReader["Password"] as string ?? "Undefined",
                                Codeword = sqlDataReader["Codeword"] as string ?? "Undefined",

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<User> GetByUserName(string userName)
        {
            string sql = $"select Id, UserName,Password,Codeword from Users where UserName=@username";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    SqlParameter username = new SqlParameter
                    {
                        ParameterName = "@username",
                        Value = userName,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    sqlCommand.Parameters.Add(username);

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (await sqlDataReader.ReadAsync())
                        {
                            return new User()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserName = sqlDataReader["UserName"] as string ?? "Undefined",
                                Password = sqlDataReader["Password"] as string ?? "Undefined",
                                Codeword = sqlDataReader["Codeword"] as string ?? "Undefined",

                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<bool> Update(User entity)
        {
            string query = $"UPDATE Users SET Password=@newPassword,Codeword=@Codeword,UserName=@UserName WHERE Id=@userId";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@userId", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@Codeword", SqlDbType.NVarChar).Value = entity.Codeword;
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = entity.UserName;
                    sqlCommand.Parameters.Add("@newPassword", SqlDbType.NVarChar).Value = entity.Password;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> UpdatePassword(int userId, string newPassword)
        {
            string query = $"UPDATE Users SET Password=@newPassword WHERE Id=@userId";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                    sqlCommand.Parameters.Add("@newPassword", SqlDbType.NVarChar).Value = newPassword;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}
