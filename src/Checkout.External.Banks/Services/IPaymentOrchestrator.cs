using Checkout.Services.Banks;

namespace Checkout.Services.Services
{
    public interface IPaymentOrchestrator
    {
        BankPaymentResponse ProcessPayment(BankPaymentRequest bankPaymentRequest);
    }
}
