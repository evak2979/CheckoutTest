using System;
using Checkout.Repository.Models;

namespace Checkout.Repository.Models
{
    public class PaymentReadModel
    {
        public string CardNumber { get; private set; }

        public string ExpiryDate { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public string CVV { get; private set; }

        public Guid PaymentId { get; private set; }

        public Guid MerchantId{ get; private set; }

        public string PaymentStatus { get; set; }

        public static PaymentReadModel BuildPaymentReadModel(PaymentInformation payment)
        {
            return new PaymentReadModel
            {
                CardNumber = payment.CardDetails.CardNumber,
                ExpiryDate = payment.CardDetails.ExpiryDate,
                CVV = payment.CardDetails.CVV,
                Amount = payment.Amount,
                MerchantId = payment.MerchantDetails.Id,
                PaymentId = payment.Id,
                Currency = payment.CardDetails.Currency,
                PaymentStatus = payment.PaymentStatus
            };
        }
    }
}