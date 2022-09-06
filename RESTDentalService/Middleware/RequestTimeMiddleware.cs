using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RESTDentalService.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly Stopwatch _stopWatch;
        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            if (_stopWatch.ElapsedMilliseconds > 4000)
            {
                var msg = $"Request [{context.Request.Method}] at {context.Request.Path} took {_stopWatch.ElapsedMilliseconds}";
                _logger.LogInformation(msg);
            }

            _stopWatch.Reset();
        }
    }
}
