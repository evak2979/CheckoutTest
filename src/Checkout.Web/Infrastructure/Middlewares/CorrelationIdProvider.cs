using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Checkout.Web.Infrastructure.Middlewares
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private const string DefaultHeader = "Correlation-ID";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCorrelationId()
        {
            var context = _httpContextAccessor.HttpContext;

            return context.Request.Headers.TryGetValue(DefaultHeader, out StringValues correlationId)
                ? (string)correlationId
                : Guid.NewGuid().ToString();
        }
    }
}