using Checkout.Services.Models;

namespace Checkout.Services.Banks
{
    public class BankPaymentRequest
    {
        public decimal Amount { get; set; }

        public MerchantDetails MerchantDetails { get; set; }

        public CardDetails CardDetails { get; set; }
    }
}