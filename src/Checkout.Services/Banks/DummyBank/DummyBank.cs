using System;

namespace Checkout.Services.Banks.DummyBank
{
    public sealed class DummyBank : IBank
    {
        public BankPaymentResponse ProcessPayment(BankPaymentRequest bankPaymentRequest)
        {
            if (bankPaymentRequest.Amount <= 0)
                return new BankPaymentResponse
                {
                    BankPaymentResponseStatus = BankPaymentResponseStatus.Unsuccessful,
                    PaymentId = Guid.NewGuid(),
                    Message = "Unsuccessful Payment - Random reason"
                };
            else
                return new BankPaymentResponse
                {
                    BankPaymentResponseStatus = BankPaymentResponseStatus.Successful,
                    PaymentId = Guid.NewGuid(),
                    Message = "Successful Payment"
                };
        }
    }
}