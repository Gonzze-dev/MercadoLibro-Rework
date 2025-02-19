using MercadoLibro.Features.Transaction;
using MercadoLibroDB;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.UserFeature
{
    public class UserRepository(
        TransactionDB transationDB    
    )
    {
        readonly MercadoLibroContext _context = transationDB._context;

        public async Task<User>AddAsync(User user)
        {
            await _context.User.AddAsync(user);

            return user;
        }

        public async Task<UserAuth> AddAsync(UserAuth userAuth)
        {
            await _context.UserAuth.AddAsync(userAuth);

            return userAuth;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
