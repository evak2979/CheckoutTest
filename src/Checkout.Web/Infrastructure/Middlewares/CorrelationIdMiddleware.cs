using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Checkout.Web.Infrastructure.Middlewares
{
    /// <summary>
    /// Based on: https://www.stevejgordon.co.uk/asp-net-core-correlation-ids
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ICorrelationIdProvider correlationIdProvider,
            ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _correlationIdProvider = correlationIdProvider;
            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            using (_logger.BeginScope(("correlation_id", correlationId)))
            {
                context.TraceIdentifier = correlationId;

                return _next(context);
            }
        }
    }
}