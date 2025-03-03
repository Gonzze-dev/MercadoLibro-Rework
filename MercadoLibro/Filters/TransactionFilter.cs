using MercadoLibroDB;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MercadoLibro.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionFilter(
        TransactionDB transaction
        ) : Attribute, IAsyncActionFilter
    {
        readonly TransactionDB _transaction = transaction;


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await _transaction.BeginTransactionAsync();
            var resultContext = await next();

            if (resultContext.Exception == null)
            {
                await _transaction.CommitAsync();
            }
            else
            {
                await _transaction.RollbackAsync();
            }
        }
    }
}
