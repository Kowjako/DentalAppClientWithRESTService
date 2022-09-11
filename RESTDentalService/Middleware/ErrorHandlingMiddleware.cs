using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RESTDentalService.Middleware
{
    /// <summary>
    /// Globalny middleware do przechwytywania wyjatkow po stronie API
    /// </summary>
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
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
                _logger.LogError(ex, ex.Message);
                await context.Response.WriteAsync(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                _logger.LogError(ex, ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                _logger.LogError(ex, ex.Message);
                await context.Response.WriteAsync("Wystapił błąd po stronie serwera");
            }
        }
    }
}
