using Microsoft.EntityFrameworkCore.Storage;

namespace MercadoLibroDB
{
    public class TransactionDB(
        MercadoLibroContext context
    )
    {
        public readonly MercadoLibroContext Context = context;
        IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await Context.Database.BeginTransactionAsync();
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
