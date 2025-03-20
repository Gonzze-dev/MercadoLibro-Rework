using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibro.Features.CartFeature.Repositories
{
    public class CartRepository(
        TransactionDB transactionDb
    )
    {
        readonly MercadoLibroContext _context = transactionDb.Context;

        public async Task AddAsync(Cart cart)
        {
            await _context.Cart.AddAsync(cart);
        }

        public async Task<Cart?> Get(Guid userId)
        {
            return await _context
                        .Cart
                        .FirstOrDefaultAsync(c => c.UserID == userId);
        }

        public void Remove(Cart cart)
        {
            _context.Cart.Remove(cart);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
