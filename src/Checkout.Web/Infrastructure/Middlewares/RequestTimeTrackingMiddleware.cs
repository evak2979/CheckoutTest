using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Checkout.Web.Infrastructure.Middlewares
{
    public class RequestTimeTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimeTrackingMiddleware> _logger;

        public RequestTimeTrackingMiddleware(RequestDelegate next, ILogger<RequestTimeTrackingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = new Stopwatch();
            _logger.LogInformation($"Request with correlationId {context.TraceIdentifier} started");
            stopWatch.Start();
            
            await _next(context);

            stopWatch.Stop();
            _logger.LogInformation($"Request with correlationId {context.TraceIdentifier} ended in {stopWatch.Elapsed.TotalMilliseconds} ms");
        }
    }

    public static class RequestTimeTrackingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTimeTrackingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimeTrackingMiddleware>();
        }
    }
}