namespace Checkout.Services.Banks
{
    public interface IBank
    {
        BankPaymentResponse ProcessPayment(BankPaymentRequest bankPaymentRequest);
    }
}