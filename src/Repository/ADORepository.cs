using EfSamples.Model;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EfSamples.Repository
{
    class ADORepository : IRepository
    {
        private readonly SqlConnection _sqlConnection;

        public ADORepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<User> AddUser(User user)
        {
            string sqlExpression = @"INSERT INTO Users (Login, Email) VALUES (@Login, @Email);
                                     SET @userId=SCOPE_IDENTITY();";
            await _sqlConnection.OpenAsync();
            var command = new SqlCommand(sqlExpression, _sqlConnection);
            // создаем параметр для Login
            var nameParam = new SqlParameter("@Login", user.Login);
            // добавляем параметр к команде
            command.Parameters.Add(nameParam);
            // создаем параметр для Email
            nameParam = new SqlParameter("@Email", user.Email);
            // добавляем параметр к команде
            command.Parameters.Add(nameParam);
            // параметр для userId
            var userIdParam = new SqlParameter
            {
                ParameterName = "@userId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output // выходной параметр 
            };
            command.Parameters.Add(userIdParam);

            await command.ExecuteNonQueryAsync();
            user.Id = (int)userIdParam.Value;
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            string sqlExpression = "SELECT * FROM Users";
            await _sqlConnection.OpenAsync();
            var command = new SqlCommand(sqlExpression, _sqlConnection);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows) // если есть данные
            {
                var users = new List<User>();
                while (await reader.ReadAsync()) // построчно считываем данные
                {
                    users.Add(new User()
                    {
                        Id = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Email = reader.GetString(2)
                    });

                }
                return users;
            }

            reader.Close();
            return null;
        }

        public async Task<User> GetUser(int userId)
        {
            string sqlExpression = @"SELECT TOP(1) Users.Id as UserId, Users.Login, Users.Email
                                    FROM Users WHERE Users.Id = @ID;";
            await _sqlConnection.OpenAsync();

            using var transaction = await _sqlConnection.BeginTransactionAsync(IsolationLevel.RepeatableRead);

            var command = new SqlCommand(sqlExpression, _sqlConnection);
            // создаем параметр для UsersId
            SqlParameter nameParam = new SqlParameter("@ID", userId);
            // добавляем параметр к команде
            command.Parameters.Add(nameParam);
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows) // если есть данные
            {
                if (await reader.ReadAsync()) // построчно считываем данные
                {
                    return new User()
                    {
                        Id = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Email = reader.GetString(2)
                    };
                }
            }

            reader.Close();

            await transaction.CommitAsync();
            return null;
        }

        public async Task RemoveUser(User user)
        {
            string sqlExpression = "DELETE FROM Users WHERE Users.Id = @ID;";
            await _sqlConnection.OpenAsync();
            SqlCommand command = new SqlCommand(sqlExpression, _sqlConnection);
            // создаем параметр для UsersId
            SqlParameter nameParam = new SqlParameter("@ID", user.Id);
            // добавляем параметр к команде
            command.Parameters.Add(nameParam);
            // number - кол-во удалённых записей
            int number = await command.ExecuteNonQueryAsync();
        }

        public async Task<User> UpdateUser(User user)
        {
            string sqlExpression = @"UPDATE Users SET Login=@Login, Email=@Email WHERE Users.Id = @userId;";
            await _sqlConnection.OpenAsync();
            var command = new SqlCommand(sqlExpression, _sqlConnection);
            // создаем параметр для UsersId
            SqlParameter nameParam = new SqlParameter("@userId", user.Id);
            // добавляем параметр к команде
            command.Parameters.Add(nameParam);
            // создаем параметр для Login
            nameParam = new SqlParameter("@Login", user.Login);
            // добавляем параметр к команде
            command.Parameters.Add(nameParam);
            // создаем параметр для Email
            nameParam = new SqlParameter("@Email", user.Email);
            // добавляем параметр к команде
            command.Parameters.Add(nameParam);
            // кол-во обновлённх записей
            int number = await command.ExecuteNonQueryAsync();
            return user;
        }
    }
}
