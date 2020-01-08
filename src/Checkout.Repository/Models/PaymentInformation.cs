using System;
using Checkout.Repository.Models.Base;

namespace Checkout.Repository.Models
{
    public sealed class PaymentInformation : BaseRequest
    {
        public Guid Id { get; set; }

        public CardDetails CardDetails { get; set; }

        public MerchantDetails MerchantDetails { get; set; }

        public decimal Amount { get; set; }

        public string PaymentStatus { get; set; }
    }
}