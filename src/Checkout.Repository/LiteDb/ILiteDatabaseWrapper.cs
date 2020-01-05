using Checkout.Repository.Models;

namespace Checkout.Repository.LiteDb
{
    public interface ILiteDatabaseWrapper
    {
        void Insert(PaymentInformation payment);

        PaymentInformation Get(RetrievePaymentRequest paymentRequest);
    }
}