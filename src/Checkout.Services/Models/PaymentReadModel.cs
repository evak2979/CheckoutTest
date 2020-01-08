using System;

namespace Checkout.Services.Models
{
    public sealed class PaymentReadModel
    {
        public string CardNumber { get; private set; }

        public string ExpiryDate { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public string CVV { get; private set; }

        public Guid PaymentId { get; private set; }

        public Guid MerchantId { get; private set; }

        public string PaymentStatus { get; set; }

        public static PaymentReadModel BuildPaymentReadModel(Repository.Models.PaymentReadModel payment)
        {
            return new PaymentReadModel
            {
                CardNumber = payment.CardNumber,
                ExpiryDate = payment.ExpiryDate,
                CVV = payment.CVV,
                Amount = payment.Amount,
                MerchantId = payment.MerchantId,
                PaymentId = payment.PaymentId,
                Currency = payment.Currency,
                PaymentStatus = payment.PaymentStatus
            };
        }
    }
}