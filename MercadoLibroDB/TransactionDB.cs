using MercadoLibroDB;
using Microsoft.EntityFrameworkCore.Storage;

namespace MercadoLibro.Features.Transaction
{
    public class TransactionDB(
        MercadoLibroContext context
    )
    {
        public readonly MercadoLibroContext _context = context;
        IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null) 
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
            
        }
    }
}
