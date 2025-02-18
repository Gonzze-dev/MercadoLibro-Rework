using MercadoLibro.Features.Filters;
using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Features.UserFeature
{
    public class UserRepository(
        MercadoLibroContext context    
    )
    {
        readonly MercadoLibroContext _context = context;

        [ServiceFilter(typeof(TransactionFilter))]
        public async Task<User> AddUser(User user, UserAuth userAuth)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserAuth> AddUserAuth(UserAuth userAuth)
        {

            _context.UserAuth.Add(userAuth);

            await _context.SaveChangesAsync();

            return userAuth;
        }
    }
}
