using EfSamples.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfSamples.Repository
{
    public class EFRepository : IRepository
    {
        private readonly ApplicationContext _applicationContext;

        public EFRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<User> GetUser(int userId) =>
            await _applicationContext.Users
                .AsNoTracking()
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

        public async Task<User> AddUser(User user)
        {
            var result = _applicationContext.Users.Add(user);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task RemoveUser(User user)
        {
            var _user = await GetUser(user.Id);
            
            _applicationContext.Users.Remove(_user);

            await _applicationContext.SaveChangesAsync();
        }

        public async Task<User> UpdateUser(User user)
        {
            var result = _applicationContext.Users.Update(user);

            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<User>> GetAllUsers() =>
            await _applicationContext.Users
                .AsNoTracking()
                .ToListAsync();
    }
}
