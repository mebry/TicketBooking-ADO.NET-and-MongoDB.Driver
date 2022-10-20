using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;

namespace Booking.DAL.Repositories.MsServer
{
    public class UserDetailsRepositoryMS : IUserDetailsRepository
    {
        private readonly string _connectionString;

        public UserDetailsRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<bool> Create(UserDelails entity)
        {
            string query = $"INSERT INTO UserDetails (FirstName,LastName,Patronymic,UserId) VALUES(@FirstName,@LastName," +
                $"@Patronymic,@UserId)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
                    sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = entity.LastName;
                    sqlCommand.Parameters.Add("@Patronymic", SqlDbType.NVarChar).Value = entity.Patronymic;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM UserDetails WHERE Id = @id";

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

        public async Task<List<UserDelails>> GetAll()
        {
            string sql = $"select FirstName,LastName,Patronymic,UserId from UserDetails";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<UserDelails> userDelails = new List<UserDelails>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            userDelails.Add(new UserDelails()
                            {
                                UserId = (int)sqlDataReader["UserId"],
                                FirstName = sqlDataReader["FirstName"] as string ?? "Undefined",
                                LastName = sqlDataReader["LastName"] as string ?? "Undefined",
                                Patronymic = sqlDataReader["Patronymic"] as string ?? "Undefined",
                            });
                        }
                        if (userDelails.Count > 0)
                        {
                            return userDelails;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<UserDelails> GetById(int id)
        {
            string sql = $"select FirstName,LastName,Patronymic,UserId from UserDetails where Id=@id";

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
                            return new UserDelails()
                            {
                                UserId = (int)sqlDataReader["UserId"],
                                FirstName = sqlDataReader["FirstName"] as string ?? "Undefined",
                                LastName = sqlDataReader["LastName"] as string ?? "Undefined",
                                Patronymic = sqlDataReader["Patronymic"] as string ?? "Undefined"
                            };
                        }
                    }
                }

            }

            return null;
        }

        public async Task<UserDelails> GetByUserName(string userName)
        {
            string sql = @$"SELECT UserDetails.FirstName,UserDetails.LastName,UserDetails.Patronymic,UserDetails.UserId
                        FROM UserDetails, Users
                        WHERE Users.Id = UserDetails.UserId AND Users.UserName = @UserName";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    SqlParameter username = new SqlParameter
                    {
                        ParameterName = "@UserName",
                        Value = userName,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };

                    sqlCommand.Parameters.Add(username);

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (await sqlDataReader.ReadAsync())
                        {
                            return new UserDelails()
                            {
                                UserId = (int)sqlDataReader["UserId"],
                                FirstName = sqlDataReader["FirstName"] as string ?? "Undefined",
                                LastName = sqlDataReader["LastName"] as string ?? "Undefined",
                                Patronymic = sqlDataReader["Patronymic"] as string ?? "Undefined"
                            };
                        }
                    }
                }

            }

            return null;

        }

        public async Task<bool> Update(UserDelails entity)
        {
            string query = $"UPDATE UserDetails SET FirstName=@FirstName,LastName=@LastName,Patronymic=@Patronymic WHERE UserId=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
                    sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = entity.LastName;
                    sqlCommand.Parameters.Add("@Patronymic", SqlDbType.NVarChar).Value = entity.Patronymic;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> UpdateFirstName(int userId, string firstName)
        {
            string query = $"UPDATE UserDetails SET FirstName=@FirstName WHERE UserId=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = userId;
                    sqlCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = firstName;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> UpdateLastName(int userId, string lastName)
        {
            string query = $"UPDATE UserDetails SET LastName=@LastName WHERE UserId=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = userId;
                    sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = lastName;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> UpdatePatronymic(int userId, string patronymic)
        {
            string query = $"UPDATE UserDetails SET Patronymic=@Patronymic WHERE UserId=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = userId;
                    sqlCommand.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = patronymic;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}
