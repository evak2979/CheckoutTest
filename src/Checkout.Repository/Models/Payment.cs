using System;

namespace Checkout.Repository.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public CardDetails CardDetails { get; set; }

        public MerchantDetails MerchantDetails { get; set; }
        public decimal Amount { get; set; }
    }
}