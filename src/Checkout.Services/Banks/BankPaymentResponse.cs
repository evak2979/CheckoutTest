using System;

namespace Checkout.Services.Banks
{
    public sealed class BankPaymentResponse
    {
        public Guid PaymentId { get; set; }

        public BankPaymentResponseStatus BankPaymentResponseStatus { get; set; }

        public string Message { get; set; }
    }
}