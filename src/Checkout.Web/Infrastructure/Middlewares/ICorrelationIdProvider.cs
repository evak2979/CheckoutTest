namespace Checkout.Web.Infrastructure.Middlewares
{
    public interface ICorrelationIdProvider
    {
        string GetCorrelationId();
    }
}