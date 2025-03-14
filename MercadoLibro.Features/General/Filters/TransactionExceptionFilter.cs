using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Npgsql;
using System.Data.Common;

namespace MercadoLibro.Features.General.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionExceptionFilter : Attribute, IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {
            var tException = context.Exception;

            if (tException is NpgsqlException tEsception)
            {

                var state = tEsception.SqlState;
                var nessage = tException.Message;

                context.Result = new ObjectResult($"Error in database \nSTATE: ${state} \nMESSAGE: ${nessage}")
                {
                    StatusCode = 500
                };
            }
            if (tException is DbException)
            {
                context.Result = new ObjectResult($"Error in database \nMessage: ${tException.Message}")
                {
                    StatusCode = 500
                };
            }
            if (tException is not null)
            {
                context.Result = new ObjectResult($"Internal Server Error \nMessage: ${tException.Message}")
                {
                    StatusCode = 500
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
