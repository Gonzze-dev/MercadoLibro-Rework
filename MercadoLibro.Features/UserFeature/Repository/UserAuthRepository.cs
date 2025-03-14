using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.UserFeature.Repository
{
    public class UserAuthRepository(
        TransactionDB transationDB
    )
    {
        readonly MercadoLibroContext _context = transationDB.Context;

        public async Task<UserAuth?> Get(Guid userId, string authMethod = "local")
        {
            var userAuth = await _context.UserAuth.FirstOrDefaultAsync(uAuth =>
                uAuth.UserID == userId
                && uAuth.AuthMethod == authMethod
            );

            return userAuth;
        }

        public async Task<UserAuth?> Get(Guid userId)
        {
            string AUTH_METHOD = "local";

            var userAuth = await _context.UserAuth.FirstOrDefaultAsync(uAuth =>
                uAuth.UserID == userId
                && uAuth.AuthMethod == AUTH_METHOD
            );

            return userAuth;
        }

        public async Task<bool> Exists(string authMethod, Guid userId)
        {
            var exists = await _context.UserAuth.AnyAsync(uAuth =>
                uAuth.UserID == userId
                && uAuth.AuthMethod == authMethod
            );

            return exists;
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
