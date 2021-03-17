using Dapper;
using EfSamples.Model;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfSamples.Repository
{
    public class DapperRepository : IRepository
    {
        private readonly SqlConnection _sqlConnection;

        public DapperRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<User> AddUser(User user)
        {
            //var sqlQuery = "INSERT INTO Users (Login, Email) VALUES(@Login, @Email)";
            //_sqlConnection.Execute(sqlQuery, user);

            // если мы хотим получить id добавленного пользователя
            var sqlQuery = "INSERT INTO Users (Login, Email) VALUES(@Login, @Email); SELECT CAST(SCOPE_IDENTITY() as int)";

            int? userId = (await _sqlConnection.QueryAsync<int>(sqlQuery, user)).FirstOrDefault();
            user.Id = userId.Value;
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers() =>
            (await _sqlConnection.QueryAsync<User>("SELECT * FROM Users")).ToList();

        public async Task<User> GetUser(int userId) =>
            (await _sqlConnection.QueryAsync<User>("SELECT * FROM Users WHERE Id = @userId", new { userId }))
            .FirstOrDefault();

        public async Task RemoveUser(User user) =>
            await _sqlConnection.ExecuteAsync("DELETE FROM Users WHERE Id = @id", new { id = user.Id });

        public async Task<User> UpdateUser(User user)
        {
            await _sqlConnection.ExecuteAsync("UPDATE Users SET Login = @Login, Email = @Email WHERE Id = @Id", user);

            return user;
        }
    }
}
