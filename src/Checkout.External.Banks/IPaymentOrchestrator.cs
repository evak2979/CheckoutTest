using Checkout.Services.Banks;

namespace Checkout.Services
{
    public interface IPaymentOrchestrator
    {
        BankPaymentResponse ProcessPayment(BankPaymentRequest bankPaymentRequest);
    }
}
