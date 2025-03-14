using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.UserFeature.Repository
{
    public class UserRepository(
        TransactionDB transationDB
    )
    {
        readonly MercadoLibroContext _context = transationDB.Context;

        public async Task<User?> Get(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public async Task<User?> Get(Guid id)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.User.ToListAsync();
            return users;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.User.AddAsync(user);

            return user;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
