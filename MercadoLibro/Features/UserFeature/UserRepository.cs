using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.UserFeature
{
    public class UserRepository(
        TransactionDB transationDB    
    )
    {
        readonly MercadoLibroContext _context = transationDB.Context;

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<bool> ExistsWithAuthMethod(string authMethod, Guid userId)
        {
            var exists = await _context.UserAuth.AnyAsync(uAuth => 
                uAuth.UserID == userId 
                && uAuth.AuthMethod == authMethod
            );
            
            return exists;
        }

        public async Task<UserAuth?> GetUserAuth(Guid userId, string authMethod = "local")
        {
            var userAuth = await _context.UserAuth.FirstOrDefaultAsync(uAuth => 
                uAuth.UserID == userId
                &&  uAuth.AuthMethod == authMethod
            );

            return userAuth;
        }

        public async Task<UserAuth?> GetUserAuth(Guid userId)
        {
            string LOCAL_AUTH_METHOD = "local";

            var userAuth = await _context.UserAuth.FirstOrDefaultAsync(uAuth =>
                uAuth.UserID == userId
                && uAuth.AuthMethod == LOCAL_AUTH_METHOD
            );

            return userAuth;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.User.ToListAsync();
            return users;
        }

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
