using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace RESTDentalService.Middleware
{
    /// <summary>
    /// Globalny middleware do przechwytywania wyjatkow po stronie API
    /// </summary>
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public ErrorHandlingMiddleware()
        {

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ArgumentNullException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Wystapił błąd po stronie serwera");
            }
        }
    }
}
