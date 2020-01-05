using Checkout.Repository.Models.Base;
using Checkout.Services.Models;

namespace Checkout.Services.Banks
{
    public class BankPaymentRequest : BaseRequest
    {
        public decimal Amount { get; set; }

        public MerchantDetails MerchantDetails { get; set; }

        public CardDetails CardDetails { get; set; }
    }
}