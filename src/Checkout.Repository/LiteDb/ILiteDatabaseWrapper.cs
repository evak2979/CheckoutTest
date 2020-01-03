using Checkout.Repository.Models;

namespace Checkout.Repository.LiteDb
{
    public interface ILiteDatabaseWrapper
    {
        void Insert(Payment payment);

        Payment Get(PaymentRequest paymentRequest);
    }
}