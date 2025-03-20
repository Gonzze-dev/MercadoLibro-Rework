using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.CartFeature.Repositories
{
    public class CartLineRepository(
        TransactionDB transactionDb
    )
    {
        readonly MercadoLibroContext _context = transactionDb.Context;

        public async Task AddAsync(CartLine cartLine)
        {
            await _context.CartLine.AddAsync(cartLine);
        }

        public async Task<CartLine?> Remove(Guid userID, string isbn)
        {
            CartLine? cartLine = await _context.CartLine.FirstOrDefaultAsync(cl => 
                cl.UserID == userID 
                && cl.ISBN == isbn
            );

            if (cartLine is null)
                return null;

            _context.CartLine.Remove(cartLine);

            return cartLine;
        }

        public async Task<List<CartLine>> GetAll(Guid userId)
        {
            return await _context
                        .CartLine
                        .Where(cl => cl.UserID == userId)
                        .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
