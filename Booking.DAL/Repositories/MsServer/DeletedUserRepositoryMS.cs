using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;

namespace Booking.DAL.Repositories.MsServer
{
    public class DeletedUserRepositoryMS : IDeletedUserRepository
    {
        private readonly string _connectionString;

        public DeletedUserRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> Create(DeletedUser entity)
        {
            string query = $"INSERT INTO DeletedUsers (UserId,DateTime,Reason) VALUES(@UserId,@DateTime,@Reason)";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = entity.DateTime;
                    sqlCommand.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = entity.Reason;
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string query = $@"DELETE FROM DeletedUsers WHERE Id = @id";

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

        public async Task<List<DeletedUser>> GetAll()
        {
            string sql = $"select Id,UserId,DateTime,Reason from DeletedUsers";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    List<DeletedUser> deletedUsers = new List<DeletedUser>();
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            deletedUsers.Add(new DeletedUser()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserId = (int)sqlDataReader["UserId"],
                                DateTime = (DateTime)sqlDataReader["DateTime"],
                                Reason =sqlDataReader["Reason"] as string ?? "Undefined",
                            });
                        }
                        if (deletedUsers.Count > 0)
                        {
                            return deletedUsers;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task<DeletedUser> GetById(int id)
        {
            string sql = $"select Id,UserId,DateTime,Reason from DeletedUsers where Id=@id";

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
                            return new DeletedUser()
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserId = (int)sqlDataReader["UserId"],
                                DateTime = (DateTime)sqlDataReader["DateTime"],
                                Reason = sqlDataReader["Reason"] as string ?? "Undefined",

                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> Update(DeletedUser entity)
        {
            string query = $"UPDATE DeletedUsers SET UserId=@UserId,DateTime=@DateTime,Reason=@Reason WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = entity.Id;
                    sqlCommand.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = entity.UserId;
                    sqlCommand.Parameters.Add("@DateTime", SqlDbType.DateTime).Value = entity.DateTime;
                    sqlCommand.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = entity.Reason;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }

        public async Task<bool> UpdateTheReason(int id, string reason)
        {
            string query = $"UPDATE DeletedUsers SET Reason=@Reason WHERE Id=@id";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = reason;

                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return true;
            }
        }
    }
}
