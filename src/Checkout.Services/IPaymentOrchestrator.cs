using Checkout.Services.Banks;
using Checkout.Services.Models;

namespace Checkout.Services
{
    public interface IPaymentOrchestrator
    {
        BankPaymentResponse ProcessPayment(BankPaymentRequest bankPaymentRequest);

        PaymentReadModel RetrievePayment(RetrievePaymentRequest paymentRequest);
    }
}
