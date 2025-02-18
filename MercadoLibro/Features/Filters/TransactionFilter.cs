using MercadoLibroDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Npgsql;
using System.Data.Common;

namespace MercadoLibro.Features.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionFilter(MercadoLibroContext context) : Attribute, IAsyncActionFilter
    {
        readonly MercadoLibroContext _context = context;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                await next();
                _ = transaction.CommitAsync();
            }
            catch (NpgsqlException ex)
            {
                _ = transaction.RollbackAsync();
                var state = ex.SqlState;
                var nessage = ex.Message;

                context.Result = new ObjectResult($"Error in database \nSTATE: ${state} \nMESSAGE: ${nessage}")
                {
                    StatusCode = 500
                };
            }
            catch (DbException ex)
            {
                _ = transaction.RollbackAsync();

                context.Result = new ObjectResult($"Error in database \nMessage: ${ex.Message}")
                {
                    StatusCode = 500
                };
            }
            catch (Exception ex)
            {
                _ = transaction.RollbackAsync();

                context.Result = new ObjectResult($"Internal Server Error \nMessage: ${ex.Message}")
                {
                    StatusCode = 500
                };
            }
        }
    }
}
