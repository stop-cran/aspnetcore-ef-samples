using EfSamples.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EfSamples.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(int userId);
        Task<User> AddUser(User user);
        Task RemoveUser(User user);
        Task<User> UpdateUser(User user);
    }
}
